// TODO fill in this information for your driver, then remove this line!
//
// ASCOM ObservingConditions driver for SQMeter
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM ObservingConditions interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//

using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.SQMeter.ObservingConditions
{
    //
    // This code is mostly a presentation layer for the functionality in the ObservingConditionsHardware class. You should not need to change the contents of this file very much, if at all.
    // Most customisation will be in the ObservingConditionsHardware class, which is shared by all instances of the driver, and which must handle all aspects of communicating with your device.
    //
    // Your driver's DeviceID is ASCOM.SQMeter.ObservingConditions
    //
    // The COM Guid attribute sets the CLSID for ASCOM.SQMeter.ObservingConditions
    // The COM ClassInterface/None attribute prevents an empty interface called _SQMeter from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM ObservingConditions Driver for SQMeter.
    /// </summary>
    [ComVisible(true)]
    [Guid("82859f55-6f65-485e-afad-42b99793edb7")]
    [ProgId("ASCOM.SQMeter.ObservingConditions")]
    [ServedClassName("SQMeter")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : ReferenceCountedObjectBase, IObservingConditionsV2, IDisposable
    {
        internal static string DriverProgId; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string DriverDescription; // The value is retrieved from the ServedClassName attribute in the class initialiser.

        // connectedState holds the connection state from this driver instance's perspective, as opposed to the local server's perspective, which may be different because of other client connections.
        internal bool connectedState
        {
            get
            {
                return ObservingConditionsHardware.connectedState;
            }
            set
            {
                ObservingConditionsHardware.connectedState = value;
            }
        }

        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;

        private Guid uniqueId; // A unique ID for this instance of the driver

        #region Initialisation and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="SQMeter"/> class. Must be public to successfully register for COM.
        /// </summary>
        public ObservingConditions()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                DriverProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                DriverDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                // LOGGING CONFIGURATION
                // By default all driver logging will appear in Hardware log file
                // If you would like each instance of the driver to have its own log file as well, uncomment the lines below

                tl = new TraceLogger("", "SQMeter.Driver"); // Remove the leading ASCOM. from the ProgId because this will be added back by TraceLogger.
                SetTraceState();

                // Initialise the hardware if required
                ObservingConditionsHardware.InitialiseHardware();

                LogMessage("ObservingConditions", "Starting driver initialisation");
                LogMessage("ObservingConditions", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false

                // Create a unique ID to identify this driver instance
                uniqueId = Guid.NewGuid();

                LogMessage("ObservingConditions", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("ObservingConditions", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.SQMeter.ObservingConditions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Class destructor called automatically by the .NET runtime when the object is finalised in order to release resources that are NOT managed by the .NET runtime.
        /// </summary>
        /// <remarks>See the Dispose(bool disposing) remarks for further information.</remarks>
        ~ObservingConditions()
        {
            // Please do not change this code.
            // The Dispose(false) method is called here just to release unmanaged resources. Managed resources will be dealt with automatically by the .NET runtime.

            Dispose(false);
        }

        /// <summary>
        /// Deterministically dispose of any managed and unmanaged resources used in this instance of the driver.
        /// </summary>
        /// <remarks>
        /// Do not dispose of items in this method, put clean-up code in the 'Dispose(bool disposing)' method instead.
        /// </remarks>
        public void Dispose()
        {
            // Please do not change the code in this method.

            // Release resources now.
            Dispose(disposing: true);

            // Do not add GC.SuppressFinalize(this); here because it breaks the ReferenceCountedObjectBase COM connection counting mechanic
        }

        /// <summary>
        /// Dispose of large or scarce resources created or used within this driver file
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to enable you to release finite system resources back to the operating system as soon as possible, so that other applications work as effectively as possible.
        ///
        /// NOTES
        /// 1) Do not call the ObservingConditionsHardware.Dispose() method from this method. Any resources used in the static ObservingConditionsHardware class itself,
        ///    which is shared between all instances of the driver, should be released in the ObservingConditionsHardware.Dispose() method as usual.
        ///    The ObservingConditionsHardware.Dispose() method will be called automatically by the local server just before it shuts down.
        /// 2) You do not need to release every .NET resource you use in your driver because the .NET runtime is very effective at reclaiming these resources.
        /// 3) Strong candidates for release here are:
        ///     a) Objects that have a large memory footprint (> 1Mb) such as images
        ///     b) Objects that consume finite OS resources such as file handles, synchronisation object handles, memory allocations requested directly from the operating system (NativeMemory methods) etc.
        /// 4) Please ensure that you do not return exceptions from this method
        /// 5) Be aware that Dispose() can be called more than once:
        ///     a) By the client application
        ///     b) Automatically, by the .NET runtime during finalisation
        /// 6) Because of 5) above, you should make sure that your code is tolerant of multiple calls.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        // Dispose of managed objects here

                        // Clean up the trace logger object
                        if (!(tl is null))
                        {
                            tl.Enabled = false;
                            tl.Dispose();
                            tl = null;
                        }
                    }
                    catch (Exception)
                    {
                        // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method.
                    }
                }

                try
                {
                    // Dispose of unmanaged objects, if any, here (OS handles etc.)
                }
                catch (Exception)
                {
                    // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method.
                }

                // Flag that Dispose() has already run and disposed of all resources
                disposedValue = true;
            }
        }

        #endregion Initialisation and Dispose

        // PUBLIC COM INTERFACE IObservingConditionsV2 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            try
            {
                if (connectedState) // Don't show if already connected
                {
                    MessageBox.Show("Already connected, just press OK");
                }
                else // Show dialogue
                {
                    LogMessage("SetupDialog", $"Calling SetupDialog.");
                    ObservingConditionsHardware.SetupDialog();
                    LogMessage("SetupDialog", $"Completed.");
                }
            }
            catch (Exception ex)
            {
                LogMessage("SetupDialog", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    CheckConnected($"SupportedActions");
                    ArrayList actions = ObservingConditionsHardware.SupportedActions;
                    LogMessage("SupportedActions", $"Returning {actions.Count} actions.");
                    return actions;
                }
                catch (Exception ex)
                {
                    LogMessage("SupportedActions", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public string Action(string actionName, string actionParameters)
        {
            try
            {
                CheckConnected($"Action {actionName} - {actionParameters}");
                LogMessage("", $"Calling Action: {actionName} with parameters: {actionParameters}");
                string actionResponse = ObservingConditionsHardware.Action(actionName, actionParameters);
                LogMessage("Action", $"Completed.");
                return actionResponse;
            }
            catch (Exception ex)
            {
                LogMessage("Action", $"Threw an exception: \r\n{ex}");
                throw;
            }
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
        public void CommandBlind(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBlind: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                ObservingConditionsHardware.CommandBlind(command, raw);
                LogMessage("CommandBlind", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("CommandBlind", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
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
        public bool CommandBool(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBool: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                bool commandBoolResponse = ObservingConditionsHardware.CommandBool(command, raw);
                LogMessage("CommandBlind", $"Returning: {commandBoolResponse}.");
                return commandBoolResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandBool", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
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
        public string CommandString(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandString: {command}, Raw: {raw}");
                LogMessage("CommandString", $"Calling method - Command: {command}, Raw: {raw}");
                string commandStringResponse = ObservingConditionsHardware.CommandString(command, raw);
                LogMessage("CommandString", $"Returning: {commandStringResponse}.");
                return commandStringResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandString", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Connect to the device asynchronously using Connecting as the completion variable
        /// </summary>
        public void Connect()
        {
            try
            {
                if (connectedState)
                {
                    LogMessage("Connect", "Device already connected, ignoring method");
                    return;
                }

                LogMessage("Connect", "Calling Connect");
                ObservingConditionsHardware.Connect(uniqueId);
            }
            catch (Exception ex)
            {
                LogMessage("Connect", $"Threw an exception: \r\n{ex}");
                throw;
            }
            LogMessage("Connect", $"Connect completed OK");
        }

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                try
                {
                    // Returns the driver's connection state rather than the local server's connected state, which could be different because there may be other client connections still active.
                    LogMessage("Connected Get --", connectedState.ToString());

                    return connectedState;
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    if (value == connectedState)
                    {
                        LogMessage("Connected Set", "Device already connected, ignoring Connected Set = true");
                        return;
                    }

                    if (value)
                    {
                        connectedState = true;

                        LogMessage("Connected Set", "Connecting to device...");
                        ObservingConditionsHardware.SetConnected(uniqueId, true);
                        LogMessage("Connected Set", "Connected OK");
                    }
                    else
                    {
                        connectedState = false;

                        LogMessage("Connected Set", "Disconnecting from device...");
                        ObservingConditionsHardware.SetConnected(uniqueId, false);
                        LogMessage("Connected Set", "Disconnected OK");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Set", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Completion variable for the asynchronous Connect() and Disconnect()  methods
        /// </summary>
        public bool Connecting
        {
            get
            {
                return ObservingConditionsHardware.Connecting;
            }
        }

        /// <summary>
        /// Disconnect from the device asynchronously using Connecting as the completion variable
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (!connectedState)
                {
                    LogMessage("Disconnect", "Device already disconnected, ignoring method");
                    return;
                }

                LogMessage("Disconnect", "Calling Disconnect");
                ObservingConditionsHardware.Disconnect(uniqueId);
            }
            catch (Exception ex)
            {
                LogMessage("Disconnect", $"Threw an exception: \r\n{ex}");
                throw;
            }
            LogMessage("Disconnect", $"Completed OK");
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                try
                {
                    CheckConnected($"Description");
                    string description = ObservingConditionsHardware.Description;
                    LogMessage("Description", description);
                    return description;
                }
                catch (Exception ex)
                {
                    LogMessage("Description", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverInfo = ObservingConditionsHardware.DriverInfo;
                    LogMessage("DriverInfo", driverInfo);
                    return driverInfo;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverInfo", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public string DriverVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverVersion = ObservingConditionsHardware.DriverVersion;
                    LogMessage("DriverVersion", driverVersion);
                    return driverVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    short interfaceVersion = ObservingConditionsHardware.InterfaceVersion;
                    LogMessage("InterfaceVersion", interfaceVersion.ToString());
                    return interfaceVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("InterfaceVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string name = ObservingConditionsHardware.Name;
                    LogMessage("Name Get", name);
                    return name;
                }
                catch (Exception ex)
                {
                    LogMessage("Name", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion Common properties and methods.

        #region IObservingConditions Implementation

        /// <summary>
        /// Gets and sets the time period over which observations wil be averaged
        /// </summary>
        public double AveragePeriod
        {
            get
            {
                try
                {
                    CheckConnected("AveragePeriod Get");
                    double averageperiod = ObservingConditionsHardware.AveragePeriod;
                    LogMessage("AveragePeriod Get", averageperiod.ToString());
                    return averageperiod;
                }
                catch (Exception ex)
                {
                    LogMessage("AveragePeriod Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    CheckConnected("AveragePeriod Get");
                    LogMessage("AveragePeriod Set", value.ToString());
                    ObservingConditionsHardware.AveragePeriod = value;
                }
                catch (Exception ex)
                {
                    LogMessage("AveragePeriod Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        public double CloudCover
        {
            get
            {
                try
                {
                    CheckConnected("CloudCover");
                    double cloudCover = ObservingConditionsHardware.CloudCover;
                    LogMessage("CloudCover", cloudCover.ToString());
                    return cloudCover;
                }
                catch (Exception ex)
                {
                    LogMessage("CloudCover", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Return the device's state in one call
        /// </summary>
        public IStateValueCollection DeviceState
        {
            get
            {
                try
                {
                    CheckConnected("DeviceState");

                    // Create an array list to hold the IStateValue entries
                    List<IStateValue> deviceState = new List<IStateValue>();

                    // Add one entry for each operational state, if possible
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.CloudCover), CloudCover)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.DewPoint), DewPoint)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Humidity), Humidity)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Pressure), Pressure)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.RainRate), RainRate)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyBrightness), SkyBrightness)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyQuality), SkyQuality)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.SkyTemperature), SkyTemperature)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.StarFWHM), StarFWHM)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.Temperature), Temperature)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindDirection), WindDirection)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindSpeed), WindSpeed)); } catch { }
                    try { deviceState.Add(new StateValue(nameof(IObservingConditionsV2.WindGust), WindGust)); } catch { }
                    try { deviceState.Add(new StateValue(DateTime.Now)); } catch { }

                    // Return the overall device state
                    return new StateValueCollection(deviceState);
                }
                catch (Exception ex)
                {
                    LogMessage("DeviceState", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory in deg C
        /// </summary>
        public double DewPoint
        {
            get
            {
                try
                {
                    CheckConnected("DewPoint");
                    double dewPoint = ObservingConditionsHardware.DewPoint;
                    LogMessage("DewPoint", dewPoint.ToString());
                    return dewPoint;
                }
                catch (Exception ex)
                {
                    LogMessage("DewPoint", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Atmospheric relative humidity at the observatory in percent
        /// </summary>
        public double Humidity
        {
            get
            {
                try
                {
                    CheckConnected("Humidity");
                    double humidity = ObservingConditionsHardware.Humidity;
                    LogMessage("Humidity", humidity.ToString());
                    return humidity;
                }
                catch (Exception ex)
                {
                    LogMessage("Humidity", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory in hectoPascals (hPa)
        /// </summary>
        public double Pressure
        {
            get
            {
                try
                {
                    CheckConnected("Pressure");
                    double period = ObservingConditionsHardware.Pressure;
                    LogMessage("Pressure", period.ToString());
                    return period;
                }
                catch (Exception ex)
                {
                    LogMessage("Pressure", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Rain rate at the observatory, in millimeters per hour
        /// </summary>
        public double RainRate
        {
            get
            {
                try
                {
                    CheckConnected("RainRate");
                    double rainRate = ObservingConditionsHardware.RainRate;
                    LogMessage("RainRate", rainRate.ToString());
                    return rainRate;
                }
                catch (Exception ex)
                {
                    LogMessage("RainRate", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Forces the driver to immediately query its attached hardware to refresh sensor
        /// values
        /// </summary>
        public void Refresh()
        {
            try
            {
                CheckConnected("Refresh");
                LogMessage("Refresh", $"Calling method.");
                ObservingConditionsHardware.Refresh();
                LogMessage("Refresh", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("Refresh", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="propertyName">Name of the property whose sensor description is required</param>
        /// <returns>The sensor description string</returns>
        public string SensorDescription(string propertyName)
        {
            try
            {
                CheckConnected("SensorDescription");
                LogMessage("SensorDescription", $"Calling method.");
                string sensorDescription = ObservingConditionsHardware.SensorDescription(propertyName);
                LogMessage("SensorDescription", $"{sensorDescription}");
                return sensorDescription;
            }
            catch (Exception ex)
            {
                LogMessage("SensorDescription", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Sky brightness at the observatory, in Lux (lumens per square meter)
        /// </summary>
        public double SkyBrightness
        {
            get
            {
                try
                {
                    CheckConnected("SkyBrightness");
                    double skyBrightness = ObservingConditionsHardware.SkyBrightness;
                    LogMessage("SkyBrightness", skyBrightness.ToString());
                    return skyBrightness;
                }
                catch (Exception ex)
                {
                    LogMessage("SkyBrightness", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sky quality at the observatory, in magnitudes per square arc-second
        /// </summary>
        public double SkyQuality
        {
            get
            {
                try
                {
                    CheckConnected("SkyQuality");
                    double skyQuality = ObservingConditionsHardware.SkyQuality;
                    LogMessage("SkyQuality", skyQuality.ToString());
                    return skyQuality;
                }
                catch (Exception ex)
                {
                    LogMessage("SkyQuality", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Seeing at the observatory, measured as the average star full width half maximum (FWHM in arc secs)
        /// </summary>
        public double StarFWHM
        {
            get
            {
                try
                {
                    CheckConnected("StarFWHM");
                    double starFwhm = ObservingConditionsHardware.StarFWHM;
                    LogMessage("StarFWHM", starFwhm.ToString());
                    return starFwhm;
                }
                catch (Exception ex)
                {
                    LogMessage("StarFWHM", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sky temperature at the observatory in deg C
        /// </summary>
        public double SkyTemperature
        {
            get
            {
                try
                {
                    CheckConnected("SkyTemperature");
                    double skyTemperature = ObservingConditionsHardware.SkyTemperature;
                    LogMessage("SkyTemperature", skyTemperature.ToString());
                    return skyTemperature;
                }
                catch (Exception ex)
                {
                    LogMessage("SkyTemperature", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Temperature at the observatory in deg C
        /// </summary>
        public double Temperature
        {
            get
            {
                try
                {
                    CheckConnected("Temperature");
                    double temperature = ObservingConditionsHardware.Temperature;
                    LogMessage("Temperature", temperature.ToString());
                    return temperature;
                }
                catch (Exception ex)
                {
                    LogMessage("Temperature", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="propertyName">Name of the property whose time since last update Is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        public double TimeSinceLastUpdate(string propertyName)
        {
            try
            {
                CheckConnected("TimeSinceLastUpdate");
                LogMessage("TimeSinceLastUpdate", $"Calling method.");
                ObservingConditionsHardware.TimeSinceLastUpdate(propertyName);
                double timeSincelastUpdate = ObservingConditionsHardware.TimeSinceLastUpdate(propertyName);
                LogMessage("TimeSinceLastUpdate", $"{timeSincelastUpdate}");
                return timeSincelastUpdate;
            }
            catch (Exception ex)
            {
                LogMessage("TimeSinceLastUpdate", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Wind direction at the observatory in degrees
        /// </summary>
        public double WindDirection
        {
            get
            {
                try
                {
                    CheckConnected("WindDirection");
                    double windDirection = ObservingConditionsHardware.WindDirection;
                    LogMessage("WindDirection", windDirection.ToString());
                    return windDirection;
                }
                catch (Exception ex)
                {
                    LogMessage("WindDirection", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
        /// </summary>
        public double WindGust
        {
            get
            {
                try
                {
                    CheckConnected("WindGust");
                    double windGust = ObservingConditionsHardware.WindGust;
                    LogMessage("WindGust", windGust.ToString());
                    return windGust;
                }
                catch (Exception ex)
                {
                    LogMessage("WindGust", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Wind speed at the observatory in m/s
        /// </summary>
        public double WindSpeed
        {
            get
            {
                try
                {
                    CheckConnected("WindSpeed");
                    double windSpeed = ObservingConditionsHardware.WindSpeed;
                    LogMessage("WindSpeed", windSpeed.ToString());
                    return windSpeed;
                }
                catch (Exception ex)
                {
                    LogMessage("WindSpeed", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion IObservingConditions Implementation

        #region Private properties and methods

        // Useful properties and methods that can be used as required to help with driver development

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!connectedState)
            {
                throw new NotConnectedException($"{DriverDescription} ({DriverProgId}) is not connected: {message}");
            }
        }

        /// <summary>
        /// Log helper function that writes to the driver or local server loggers as required
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogMessage(string identifier, string message)
        {
            // This code is currently set to write messages to an individual driver log AND to the shared hardware log.

            // Write to the individual log for this specific instance (if enabled by the driver having a TraceLogger instance)
            if (tl != null)
            {
                tl.LogMessageCrLf(identifier, message); // Write to the individual driver log
            }

            // Write to the common hardware log shared by all running instances of the driver.
            ObservingConditionsHardware.LogMessage(identifier, message); // Write to the local server logger
        }

        /// <summary>
        /// Read the trace state from the driver's Profile and enable / disable the trace log accordingly.
        /// </summary>
        private void SetTraceState()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(DriverProgId, ObservingConditionsHardware.traceStateProfileName, string.Empty, ObservingConditionsHardware.traceStateDefault));
            }
        }

        #endregion Private properties and methods
    }
}