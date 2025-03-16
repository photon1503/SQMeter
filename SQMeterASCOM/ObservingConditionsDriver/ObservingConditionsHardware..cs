//
// ASCOM ObservingConditions hardware class for SQMeter
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM ObservingConditions interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.NOVAS;
using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCOM.SQMeter.ObservingConditions
{
    /// <summary>
    /// ASCOM ObservingConditions hardware class for SQMeter.
    /// </summary>
    [HardwareClass()] // Class attribute flag this as a device hardware class that needs to be disposed by the local server when it exits.
    internal static class ObservingConditionsHardware
    {
        // Constants used for Profile persistence
        internal const string comPortProfileName = "COM Port";

        internal const string comPortDefault = "COM1";
        internal const string traceStateProfileName = "Trace Level";
        internal const string traceStateDefault = "true";

        private static string DriverProgId = ""; // ASCOM DeviceID (COM ProgID) for this driver, the value is set by the driver's class initialiser.
        private static string DriverDescription = ""; // The value is set by the driver's class initialiser.
        internal static string comPort; // COM port name (if required)
        internal static int pollingInterval; // Polling interval in milliseconds
        internal static int averagePeriod; // Time period over which observations wil be averaged
        public static bool connectedState; // Local server's connected state
        private static bool connecting; // Completion variable for use with the Connect and Disconnect methods
        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.

        //private static Serial _serial;
        internal static Util utilities; // ASCOM Utilities object for use as required

        internal static AstroUtils astroUtilities; // ASCOM AstroUtilities object for use as required
        internal static TraceLogger tl; // Local server's trace logger object for diagnostic log with information that you specify

        private static List<Guid> uniqueIds = new List<Guid>(); // List of driver instance unique IDs

        //SQM properties
        // a,,full:65207,ir:13388,vis:51819,mag:9.993,dmpsas:0.00,integration:600.00,gain:9876.00,niter:1,lux:192.837417,temp:22.09,hum:63.36,pres:1432.28

        private static double _full;
        private static double _ir;
        private static double _vis;
        private static double _mag;

        private static double _dmpsas;
        private static double _integration;

        private static double _gain;
        private static double _niter;
        private static double _lux;
        private static double _temp;
        private static double _hum;
        private static double _pres;

        // average values

        private static double _avgFull;
        private static double _avgIr;
        private static double _avgVis;
        private static double _avgMag;
        private static double _avgDmpsas;
        private static double _avgIntegration;
        private static double _avgGain;
        private static double _avgNiter;
        private static double _avgLux;
        private static double _avgTemp;
        private static double _avgHum;
        private static double _avgPres;

        private class TimestampedValue
        {
            public DateTime Timestamp { get; set; }
            public double Value { get; set; }
        }

        private static List<TimestampedValue> _fullValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _irValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _visValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _magValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _dmpsasValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _integrationValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _gainValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _niterValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _luxValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _tempValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _humValues = new List<TimestampedValue>();
        private static List<TimestampedValue> _presValues = new List<TimestampedValue>();

        private static DateTime _lastUpdate = DateTime.MinValue;

        //timer for refreshing the values
        private static System.Timers.Timer _timer;

        private static bool isTimerRunning = false;

        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static ObservingConditionsHardware()
        {
            try
            {
                // Create the hardware trace logger in the static initialiser.
                // All other initialisation should go in the InitialiseHardware method.
                tl = new TraceLogger("", "SQMeter.Hardware");

                // DriverProgId has to be set here because it used by ReadProfile to get the TraceState flag.
                DriverProgId = ObservingConditions.DriverProgId; // Get this device's ProgID so that it can be used to read the Profile configuration values

                // ReadProfile has to go here before anything is written to the log because it loads the TraceLogger enable / disable state.
                ReadProfile(); // Read device configuration from the ASCOM Profile store, including the trace state

                LogMessage("ObservingConditionsHardware", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("ObservingConditionsHardware", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"ObservingConditionsHardware - {ex.Message}\r\n{ex}", $"Exception creating {ObservingConditions.DriverProgId}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void InitialiseHardware()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("InitialiseHardware", $"Start.");

            // Add any code that you want to run every time a client connects to your driver here

            // Add any code that you only want to run when the first client connects in the if (runOnce == false) block below
            if (runOnce == false)
            {
                LogMessage("InitialiseHardware", $"Starting one-off initialisation.");

                DriverDescription = ObservingConditions.DriverDescription; // Get this device's Chooser description

                LogMessage("InitialiseHardware", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false
                utilities = new Util(); //Initialise ASCOM Utilities object
                astroUtilities = new AstroUtils(); // Initialise ASCOM Astronomy Utilities object

                LogMessage("InitialiseHardware", "Completed basic initialisation");

                // Add your own "one off" device initialisation here e.g. validating existence of hardware and setting up communications
                // If you are using a serial COM port you will find the COM port name selected by the user through the setup dialogue in the comPort variable.

                SharedResources.SharedSerial.PortName = comPort;
                SharedResources.SharedSerial.Speed = SerialSpeed.ps9600;
                if (pollingInterval < 2000) pollingInterval = 2000;

                SharedResources.SharedSerial.ReceiveTimeout = pollingInterval;
                SharedResources.SharedSerial.Connected = true;

                connectedState = initSQM();

                LogMessage("InitialiseHardware", $"Connecting to hardware.");

                LogMessage("InitialiseHardware", $"One-off initialisation complete.");
                runOnce = true; // Set the flag to ensure that this code is not run again
            }
        }

        private static bool initSQM()
        {
            SharedResources.SharedSerial.Connected = true;

            // clear any buffered data
            SharedResources.SharedSerial.ClearBuffers();

            //receive the welcome message
            string welcome = SharedResources.SharedSerial.ReceiveTerminated("\r\n");

            // send "ix" to the device to get the current value
            SharedResources.SharedSerial.Transmit("ix");
            string response = SharedResources.SharedSerial.ReceiveTerminated("\r\n");

            if (!response.StartsWith("i,"))
            {
                throw new ASCOM.InvalidValueException($"Unexpected response from device: {response}");
            }
            LogMessage("initSQM", $"Response from device: {response}");
            return true;
        }

        // PUBLIC COM INTERFACE IObservingConditionsV2 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public static void SetupDialog()
        {
            // Don't permit the setup dialogue if already connected
            if (IsConnected)
                MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                LogMessage("SupportedActions Get", "Returning empty ArrayList");
                return new ArrayList();
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public static string Action(string actionName, string actionParameters)
        {
            LogMessage("Action", $"Action {actionName}, parameters {actionParameters} is not implemented");
            throw new ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public static void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new MethodNotImplementedException($"CommandBlind - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public static bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            throw new MethodNotImplementedException($"CommandBool - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public static string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new MethodNotImplementedException($"CommandString - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        ///
        /// Do not call this method from the Dispose method in your driver class.
        ///
        /// This is because this hardware class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called
        /// automatically by the  local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged
        /// resources in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// For the same reason, do not call the SharedResources.Dispose() method from this method. Any resources used in the static shared resources class
        /// itself should be released in the SharedResources.Dispose() method as usual. The SharedResources.Dispose() method will be called automatically
        /// by the local server just before it shuts down.
        ///
        /// </remarks>
        public static void Dispose()
        {
            try { LogMessage("Dispose", $"Disposing of assets and closing down."); } catch { }

            try
            {
                // Clean up the trace logger and utility objects
                tl.Enabled = false;
                tl.Dispose();
                tl = null;
            }
            catch { }

            try
            {
                utilities.Dispose();
                utilities = null;
            }
            catch { }

            try
            {
                astroUtilities.Dispose();
                astroUtilities = null;
            }
            catch { }
        }

        /// <summary>
        /// Connect to the hardware if not already connected
        /// </summary>
        /// <param name="uniqueId">Unique ID identifying the calling driver instance.</param>
        /// <remarks>
        /// The unique ID is stored to record that the driver instance is connected and to ensure that multiple calls from the same driver are ignored.
        /// If this is the first driver instance to connect, the physical hardware link to the device is established
        /// </remarks>
        public static void Connect(Guid uniqueId)
        {
            LogMessage("Connect", $"Device instance unique ID: {uniqueId}");

            // Check whether this driver instance has already connected
            if (uniqueIds.Contains(uniqueId)) // Instance already connected
            {
                // Ignore the request, the unique ID is already in the list
                LogMessage("Connect", $"Ignoring request to connect because the device is already connected.");
                return;
            }

            // Set the connection in progress flag
            connecting = true;

            // Driver instance not yet connected, so start a task to connect to the device hardware and return while the task runs in the background
            // Discard the returned task value because this a "fire and forget" task
            LogMessage("Connect", $"Starting Connect task...");
            _ = Task.Run(() =>
            {
                try
                {
                    // Set the Connected state to true, waiting until it completes
                    LogMessage("ConnectTask", $"Setting connection state to true");
                    SetConnected(uniqueId, true);
                    LogMessage("ConnectTask", $"Connected set true");
                }
                catch (Exception ex)
                {
                    LogMessage("ConnectTask", $"Exception - {ex.Message}\r\n{ex}");
                    throw;
                }
                finally
                {
                    connecting = false;
                    LogMessage("ConnectTask", $"Connecting set false");
                }
            });
            LogMessage("Connect", $"Connect task started OK");
        }

        /// <summary>
        /// Disconnect from the device asynchronously using Connecting as the completion variable
        /// </summary>
        /// <param name="uniqueId">Unique ID identifying the calling driver instance.</param>
        /// <remarks>
        /// The list of connected driver instance IDs is queried to determine whether this driver instance is connected and, if so, it is removed from the connection list.
        /// The unique ID ensures that multiple calls from the same driver are ignored.
        /// If this is the last connected driver instance, the physical link to the device hardware is disconnected.
        /// </remarks>
        public static void Disconnect(Guid uniqueId)
        {
            LogMessage("Disconnect", $"Device instance unique ID: {uniqueId}");

            // Check whether this driver instance has already disconnected
            if (!uniqueIds.Contains(uniqueId)) // Instance already disconnected
            {
                // Ignore the request, the unique ID is already removed from the list
                LogMessage("Disconnect", $"Ignoring request to disconnect because the device is already disconnected.");
                return;
            }

            // Set the Disconnect in progress flag
            connecting = true;

            // Start a task to disconnect from the device hardware and return while the task runs in the background
            // Discard the returned task value because this a "fire and forget" task
            LogMessage("Disconnect", $"Starting Disconnect task...");
            _ = Task.Run(() =>
            {
                try
                {
                    // Set the Connected state to false, waiting until it completes
                    LogMessage("DisconnectTask", $"Setting connection state to false");
                    SetConnected(uniqueId, false);
                    LogMessage("DisconnectTask", $"Connected set false");
                }
                catch (Exception ex)
                {
                    LogMessage("DisconnectTask", $"Exception - {ex.Message}\r\n{ex}");
                    throw;
                }
                finally
                {
                    connecting = false;
                    LogMessage("DisconnectTask", $"Connecting set false");
                }
            });

            LogMessage("Disconnect", $"Disconnect task started OK");
        }

        /// <summary>
        /// Completion variable for the asynchronous Connect() and Disconnect()  methods
        /// </summary>
        public static bool Connecting
        {
            get
            {
                return connecting;
            }
        }

        /// <summary>
        /// Synchronously connect to or disconnect from the hardware
        /// </summary>
        /// <param name="uniqueId">Driver's unique ID</param>
        /// <param name="newState">New state: Connected or Disconnected</param>
        public static void SetConnected(Guid uniqueId, bool newState)
        {
            // Check whether we are connecting or disconnecting
            LogMessage("SetConnected", $"Unique ID: {uniqueId}, New state: {newState}");
            if (newState) // We are connecting
            {
                // Check whether this driver instance has already connected
                if (uniqueIds.Contains(uniqueId)) // Instance already connected
                {
                    // Ignore the request, the unique ID is already in the list
                    LogMessage("SetConnected", $"Ignoring request to connect because the device is already connected.");
                }
                else // Instance not already connected, so connect it
                {
                    // Check whether this is the first connection to the hardware
                    if (uniqueIds.Count == 0) // This is the first connection to the hardware so initiate the hardware connection
                    {
                        //
                        // Add hardware connect logic here
                        //
                        //SharedResources.SharedSerial.Connected = true;
                        //initSQM();

                        Refresh();

                        //start timer
                        _timer = new System.Timers.Timer(5000);
                        _timer.Elapsed += (s, e) =>
                        {
                            Refresh();
                        };
                        _timer.Start();
                    }
                    else // Other device instances are connected so the hardware is already connected
                    {
                        // Since the hardware is already connected no action is required
                        LogMessage("SetConnected", $"Hardware already connected.");
                    }
                    connectedState = true;

                    // The hardware either "already was" or "is now" connected, so add the driver unique ID to the connected list
                    uniqueIds.Add(uniqueId);
                    LogMessage("SetConnected", $"Unique id {uniqueId} added to the connection list.");
                }
            }
            else // We are disconnecting
            {
                // Check whether this driver instance has already disconnected
                if (!uniqueIds.Contains(uniqueId)) // Instance not connected so ignore request
                {
                    // Ignore the request, the unique ID is not in the list
                    LogMessage("SetConnected", $"Ignoring request to disconnect because the device is already disconnected.");
                }
                else // Instance currently connected so disconnect it
                {
                    // Remove the driver unique ID to the connected list
                    uniqueIds.Remove(uniqueId);
                    LogMessage("SetConnected", $"Unique id {uniqueId} removed from the connection list.");

                    // Check whether there are now any connected driver instances
                    if (uniqueIds.Count == 0) // There are no connected driver instances so disconnect from the hardware
                    {
                        //
                        // Add hardware disconnect logic here
                        //
                        _timer.Stop();
                        //SharedResources.SharedSerial.Connected = false;
                    }
                    else // Other device instances are connected so do not disconnect the hardware
                    {
                        // No action is required
                        LogMessage("SetConnected", $"Hardware already connected.");
                    }
                }
                connectedState = false;
            }

            // Log the current connected state
            LogMessage("SetConnected", $"Currently connected driver ids:");
            foreach (Guid id in uniqueIds)
            {
                LogMessage("SetConnected", $" ID {id} is connected");
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public static string Description
        {
            get
            {
                LogMessage("Description Get", DriverDescription);
                return DriverDescription;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public static string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                string driverInfo = $"SQMeter for Arduino. Version: {version.Major}.{version.Minor}";
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public static string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = $"{version.Major}.{version.Minor}";
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public static short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public static string Name
        {
            get
            {
                string name = "SQMeter";
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion Common properties and methods.

        #region IObservingConditions Implementation

        // Time and wind speed values
        private static Dictionary<DateTime, double> winds = new Dictionary<DateTime, double>();

        /// <summary>
        /// Gets and sets the time period over which observations wil be averaged
        /// </summary>
        internal static double AveragePeriod
        {
            get
            {
                return averagePeriod;
            }
            set
            {
                averagePeriod = (int)value;
            }
        }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        internal static double CloudCover
        {
            get
            {
                LogMessage("CloudCover", "get - not implemented");
                throw new PropertyNotImplementedException("CloudCover", false);
            }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory in deg C
        /// </summary>
        internal static double DewPoint
        {
            get
            {
                // calculate dewpoint from _temp and _hum
                double a = 17.271;
                double b = 237.7;
                double alpha = ((a * _temp) / (b + _temp)) + Math.Log(_hum / 100.0);
                double dew = (b * alpha) / (a - alpha);
                return dew;
            }
        }

        /// <summary>
        /// Atmospheric relative humidity at the observatory in percent
        /// </summary>
        internal static double Humidity
        {
            get
            {
                return _hum;
            }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory in hectoPascals (mB)
        /// </summary>
        internal static double Pressure
        {
            get
            {
                return _pres;
            }
        }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        internal static double RainRate
        {
            get
            {
                LogMessage("RainRate", "get - not implemented");
                throw new PropertyNotImplementedException("RainRate", false);
            }
        }

        /// <summary>
        /// Forces the driver to immediately query its attached hardware to refresh sensor
        /// values
        /// </summary>
        internal static void Refresh()
        {
            if (isTimerRunning)
            {
                LogMessage("Refresh", "Timer is already running");
                return;
            }

            isTimerRunning = true;

            try
            {
                LogMessage("Refresh", "Refreshing sensor values");
                SharedResources.SharedSerial.Transmit("ax");
                string response = SharedResources.SharedSerial.ReceiveTerminated("\r\n");

                //remove trailing \r\n
                response = response.Substring(0, response.Length - 2);

                LogMessage("Refresh", $"Response: {response}");
                _lastUpdate = DateTime.Now;

                if (response != null)
                {
                    if (response.StartsWith("a,"))

                    {
                        // a,,full:65207,ir:13388,vis:51819,mag:9.993,dmpsas:0.00,integration:600.00,gain:9876.00,niter:1,lux:192.837417,temp:22.09,hum:63.36,pres:1432.28
                        string[] parts = response.Split(',');
                        foreach (string part in parts)
                        {
                            string[] kv = part.Split(':');
                            if (kv.Length == 2)
                            {
                                double value;
                                if (!double.TryParse(kv[1], NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                                {
                                    LogMessage("Refresh", $"Failed to parse value for {kv[0]}: {kv[1]}");
                                    continue;
                                }

                                var timestampedValue = new TimestampedValue { Timestamp = DateTime.Now, Value = value };

                                switch (kv[0])
                                {
                                    case "full":
                                        _full = value;
                                        _fullValues.Add(timestampedValue);
                                        break;

                                    case "ir":
                                        _ir = value;
                                        _irValues.Add(timestampedValue);
                                        break;

                                    case "vis":
                                        _vis = value;
                                        _visValues.Add(timestampedValue);
                                        break;

                                    case "mag":
                                        _mag = value;
                                        _magValues.Add(timestampedValue);
                                        break;

                                    case "dmpsas":
                                        _dmpsas = value;
                                        _dmpsasValues.Add(timestampedValue);
                                        break;

                                    case "integration":
                                        _integration = value;
                                        _integrationValues.Add(timestampedValue);
                                        break;

                                    case "gain":
                                        _gain = value;
                                        _gainValues.Add(timestampedValue);
                                        break;

                                    case "niter":
                                        _niter = value;
                                        _niterValues.Add(timestampedValue);
                                        break;

                                    case "lux":
                                        _lux = value;
                                        _luxValues.Add(timestampedValue);
                                        break;

                                    case "temp":
                                        _temp = value;
                                        _tempValues.Add(timestampedValue);
                                        break;

                                    case "hum":
                                        _hum = value;
                                        _humValues.Add(timestampedValue);
                                        break;

                                    case "pres":
                                        _pres = value;
                                        _presValues.Add(timestampedValue);
                                        break;
                                }
                            }
                        }

                        CalculateAverages();
                    }
                }
            }
            catch (Exception e)
            {
                LogMessage("Refresh", $"Exception refreshing sensor values {e.Message} {e.InnerException}");
                // throw exception to caller
                throw new Exception(Name + " - Error refreshing sensor values", e);
            }
            finally
            {
                isTimerRunning = false;
            }
        }

        private static void CalculateAverages()
        {
            DateTime cutoff = DateTime.Now - TimeSpan.FromMilliseconds(averagePeriod);
            // Clean the lists by removing old values
            _fullValues.RemoveAll(v => v.Timestamp < cutoff);
            _irValues.RemoveAll(v => v.Timestamp < cutoff);
            _visValues.RemoveAll(v => v.Timestamp < cutoff);
            _magValues.RemoveAll(v => v.Timestamp < cutoff);
            _dmpsasValues.RemoveAll(v => v.Timestamp < cutoff);
            _integrationValues.RemoveAll(v => v.Timestamp < cutoff);
            _gainValues.RemoveAll(v => v.Timestamp < cutoff);
            _niterValues.RemoveAll(v => v.Timestamp < cutoff);
            _luxValues.RemoveAll(v => v.Timestamp < cutoff);
            _tempValues.RemoveAll(v => v.Timestamp < cutoff);
            _humValues.RemoveAll(v => v.Timestamp < cutoff);
            _presValues.RemoveAll(v => v.Timestamp < cutoff);

            _avgFull = CalculateAverage(_fullValues, cutoff);
            _avgIr = CalculateAverage(_irValues, cutoff);
            _avgVis = CalculateAverage(_visValues, cutoff);
            _avgMag = CalculateAverage(_magValues, cutoff);
            _avgDmpsas = CalculateAverage(_dmpsasValues, cutoff);
            _avgIntegration = CalculateAverage(_integrationValues, cutoff);
            _avgGain = CalculateAverage(_gainValues, cutoff);
            _avgNiter = CalculateAverage(_niterValues, cutoff);
            _avgLux = CalculateAverage(_luxValues, cutoff);
            _avgTemp = CalculateAverage(_tempValues, cutoff);
            _avgHum = CalculateAverage(_humValues, cutoff);
            _avgPres = CalculateAverage(_presValues, cutoff);
        }

        private static double CalculateAverage(List<TimestampedValue> values, DateTime cutoff)
        {
            var recentValues = values.Where(v => v.Timestamp >= cutoff).Select(v => v.Value).ToList();
            return recentValues.Count > 0 ? recentValues.Average() : 0;
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="propertyName">Name of the property whose sensor description is required</param>
        /// <returns>The sensor description string</returns>
        internal static string SensorDescription(string propertyName)
        {
            switch (propertyName.Trim().ToLowerInvariant())
            {
                case "averageperiod":
                    return "Average period in hours, immediate values are only available";

                case "cloudcover":
                case "dewpoint":
                case "humidity":
                case "pressure":
                case "rainrate":
                case "skybrightness":
                case "skyquality":
                case "skytemperature":
                case "starfwhm":
                case "temperature":
                case "winddirection":
                case "windgust":
                case "windspeed":
                    // Throw an exception on the properties that are not implemented
                    LogMessage("SensorDescription", $"Property {propertyName} is not implemented");
                    throw new MethodNotImplementedException($"SensorDescription - Property {propertyName} is not implemented");
                default:
                    LogMessage("SensorDescription", $"Invalid sensor name: {propertyName}");
                    throw new InvalidValueException($"SensorDescription - Invalid property name: {propertyName}");
            }
        }

        /// <summary>
        /// Sky brightness at the observatory
        /// </summary>
        internal static double SkyBrightness
        {
            get
            {
                return _lux;
            }
        }

        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        internal static double SkyQuality
        {
            get
            {
                return _mag;
            }
        }

        /// <summary>
        /// Seeing at the observatory
        /// </summary>
        internal static double StarFWHM
        {
            get
            {
                LogMessage("StarFWHM", "get - not implemented");
                throw new PropertyNotImplementedException("StarFWHM", false);
            }
        }

        /// <summary>
        /// Sky temperature at the observatory in deg C
        /// </summary>
        internal static double SkyTemperature
        {
            get
            {
                LogMessage("SkyTemperature", "get - not implemented");
                throw new PropertyNotImplementedException("SkyTemperature", false);
            }
        }

        /// <summary>
        /// Temperature at the observatory in deg C
        /// </summary>
        internal static double Temperature
        {
            get
            {
                return _temp;
            }
        }

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="propertyName">Name of the property whose time since last update Is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        internal static double TimeSinceLastUpdate(string propertyName)
        {
            // Test for an empty property name, if found, return the time since the most recent update to any sensor
            if (!string.IsNullOrEmpty(propertyName))
            {
                switch (propertyName.Trim().ToLowerInvariant())
                {
                    // Return the time for properties that are implemented, otherwise fall through to the MethodNotImplementedException
                    case "averageperiod":
                    case "cloudcover":
                    case "dewpoint":
                    case "humidity":
                    case "pressure":
                    case "rainrate":
                    case "skybrightness":
                    case "skyquality":
                    case "skytemperature":
                    case "starfwhm":
                    case "temperature":
                    case "winddirection":
                    case "windgust":
                    case "windspeed":
                        return (DateTime.Now - _lastUpdate).TotalSeconds;

                    default:
                        LogMessage("TimeSinceLastUpdate", $"Invalid sensor name: {propertyName}");
                        throw new InvalidValueException($"TimeSinceLastUpdate - Invalid property name: {propertyName}");
                }
            }

            return (DateTime.Now - _lastUpdate).TotalSeconds;
        }

        /// <summary>
        /// Wind direction at the observatory in degrees
        /// </summary>
        internal static double WindDirection
        {
            get
            {
                LogMessage("WindDirection", "get - not implemented");
                throw new PropertyNotImplementedException("WindDirection", false);
            }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
        /// </summary>
        internal static double WindGust
        {
            get
            {
                LogMessage("WindGust", "get - not implemented");
                throw new PropertyNotImplementedException("WindGust", false);
            }
        }

        /// <summary>
        /// Wind speed at the observatory in m/s
        /// </summary>
        internal static double WindSpeed
        {
            get
            {
                LogMessage("WindSpeed", "get - not implemented");
                throw new PropertyNotImplementedException("WindSpeed", false);
            }
        }

        #endregion IObservingConditions Implementation

        #region Private methods

        #region Calculate the gust strength as the largest wind recorded over the last two minutes

        private static void UpdateGusts(double speed)
        {
            Dictionary<DateTime, double> newWinds = new Dictionary<DateTime, double>();
            var last = DateTime.Now - TimeSpan.FromMinutes(2);
            winds.Add(DateTime.Now, speed);
            var gust = 0.0;
            foreach (var item in winds)
            {
                if (item.Key > last)
                {
                    newWinds.Add(item.Key, item.Value);
                    if (item.Value > gust)
                        gust = item.Value;
                }
            }
            winds = newWinds;
        }

        #endregion Calculate the gust strength as the largest wind recorded over the last two minutes

        #endregion Private methods

        #region Private properties and methods

        // Useful methods that can be used as required to help with driver development

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private static bool IsConnected
        {
            get
            {
                return connectedState; //SharedResources.SharedSerial.Connected;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal static void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(DriverProgId, comPortProfileName, string.Empty, comPortDefault);
                averagePeriod = int.Parse(driverProfile.GetValue(DriverProgId, "avg", string.Empty, "10000"));
                pollingInterval = int.Parse(driverProfile.GetValue(DriverProgId, "polling", string.Empty, "3000"));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal static void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                driverProfile.WriteValue(DriverProgId, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(DriverProgId, comPortProfileName, comPort.ToString());
                driverProfile.WriteValue(DriverProgId, "avg", averagePeriod.ToString());
                driverProfile.WriteValue(DriverProgId, "polling", pollingInterval.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes identifier and message strings
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        internal static void LogMessage(string identifier, string message)
        {
            tl.LogMessageCrLf(identifier, message);
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            LogMessage(identifier, msg);
        }

        #endregion Private properties and methods
    }
}