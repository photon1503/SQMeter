using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

internal class SQMSerial
{
    private SerialPort _serialPort;
    private readonly SynchronizationContext _syncContext;
    private CancellationTokenSource _cts = new();
    private StringBuilder _responseBuffer = new();

    public event Action<string> DataReceived; // Event for UI updates

    public SQMSerial(string portName, int baudRate = 9600)
    {
        _syncContext = SynchronizationContext.Current;
        _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        _serialPort.DataReceived += SerialPort_DataReceived;
        try
        {
            _serialPort.Open();
        }
        catch
        {
            MessageBox.Show("Cannot open port!");
                
                
                }
    }

    // Asynchronous Send Command
    public async Task<string> SendCommandAsync(string command, int timeout = 3000)
    {
        _responseBuffer.Clear();
        _cts = new CancellationTokenSource(timeout);
        if (!_serialPort.IsOpen)
            throw new InvalidOperationException("Serial port is not open.");

        _serialPort.Write(command); // Send Command

        try
        {
            return await ReadResponseAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            return "Timeout";
        }
    }

    // Asynchronous Read Response
    private async Task<string> ReadResponseAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(100, cancellationToken); // Small delay to allow data to be received
            if (_responseBuffer.ToString().Contains("\n"))
            {
                return _responseBuffer.ToString();
            }
        }

        throw new OperationCanceledException();
    }

    // Event Handler for Incoming Data
    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            string data = _serialPort.ReadExisting();
            _responseBuffer.Append(data);

            if (_responseBuffer.ToString().Contains("\n"))
            {
                string response = _responseBuffer.ToString();
                _syncContext.Post(_ => DataReceived?.Invoke(response), null);
            }
        }
        catch (Exception ex)
        {
            _syncContext.Post(_ => DataReceived?.Invoke($"Error: {ex.Message}"), null);
        }
    }

    // Cleanup
    public void Close()
    {
        _serialPort.Close();
        _serialPort.Dispose();
        _cts.Cancel();
    }
}