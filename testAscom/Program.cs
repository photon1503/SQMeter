// This is a console application that can be used to test an ASCOM driver

// Remove the "#define UseChooser" line to bypass the code that uses the chooser to select the driver and replace it with code that accesses the driver directly via its ProgId.
// #define UseChooser

using System;
using System.Threading;

namespace ASCOM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if UseChooser
            // Choose the device
            string id = ASCOM.DriverAccess.ObservingConditions.Choose("");

            // Exit if no device was selected
            if (string.IsNullOrEmpty(id))
                return;

            // Create this device
            ASCOM.DriverAccess.ObservingConditions device = new ASCOM.DriverAccess.ObservingConditions(id);
#else
            // Create the driver class directly.
            ASCOM.DriverAccess.ObservingConditions device = new ASCOM.DriverAccess.ObservingConditions("ASCOM.SQMeter.ObservingConditions");
#endif

            device.Connect();

            // Connect to the device
            device.Connected = true;

            // Now exercise some calls that are common to all drivers.
            Console.WriteLine($"Name: {device.Name}");
            Console.WriteLine($"Description: {device.Description}");
            Console.WriteLine($"DriverInfo: {device.DriverInfo}");
            Console.WriteLine($"DriverVersion: {device.DriverVersion}");
            Console.WriteLine($"InterfaceVersion: {device.InterfaceVersion}");

            device.Connected = false;
            Console.WriteLine(device.Connected);
            device.Connected = true;
            Console.WriteLine(device.Connected);

            Console.WriteLine(device.SkyQuality);
            Console.WriteLine(device.Description);

            // Disconnect from the device
            device.Connected = false;

            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}