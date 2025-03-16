;
; Script generated by the ASCOM Driver Installer Script Generator 6.6.0.0
; Generated by photon on 12.10.2023 (UTC)
;

#define myAppVersion "1.0.0.0"
[Setup]
;SignTool=MsSign $f
;Encryption=yes
AppID={{AD20F519-CC7E-47D7-87B4-4D70951E75F2}
AppName=ASCOM SQMeter driver
AppVersion={#myAppVersion}
AppVerName="ASCOM SQMeter Driver {#myAppVersion}"
AppPublisher=photon <gerald.hitz@gmail.com>
AppPublisherURL=mailto:gerald.hitz@gmail.com
AppSupportURL=https://github.com/photon1503/SQMeter/issues
AppUpdatesURL=https://github.com/photon1503/SQMeter
VersionInfoVersion=1.0.1
MinVersion=6.1.7601
DefaultDirName="{cf}\ASCOM\ObservingConditions"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="SQMeter Setup {#myAppVersion}"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
;WizardImageFile="C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\WizardImage.bmp"
LicenseFile="C:\git\SQMeter\LICENSE"
; {cf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\ObservingConditions\SQMeter"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\ObservingConditions\SQMeter"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[Files]
Source: "C:\git\SQMeter\SQMeterASCOM\bin\Release\ASCOM.SQMeter.exe"; DestDir: "{app}"
; TODO: Add driver assemblies into the ServedClasses folder
; Require a read-me HTML to appear after installation, maybe driver's Help doc
;Source: "C:\git\ASCOMproxyhub\ReadMe.htm"; DestDir: "{app}"; Flags: isreadme
; TODO: Add other files needed by your driver here (add subfolders above)

    
; Only if driver is .NET
[Run]

; Only for .NET local-server drivers
Filename: "{app}\ASCOM.SQMeter.exe"; Parameters: "/register"



; Only if driver is .NET
[UninstallRun]
; This helps to give a clean uninstall

; Only for .NET local-server drivers
Filename: "{app}\ASCOM.SQMeter.exe"; Parameters: "/unregister"



[Code]
const
   REQUIRED_PLATFORM_VERSION = 7;    // Set this to the minimum required ASCOM Platform version for this application

//
// Function to return the ASCOM Platform's version number as a double.
//
function PlatformVersion(): Double;
var
   PlatVerString : String;
begin
   Result := 0.0;  // Initialise the return value in case we can't read the registry
   try
      if RegQueryStringValue(HKEY_LOCAL_MACHINE_32, 'Software\ASCOM','PlatformVersion', PlatVerString) then 
      begin // Successfully read the value from the registry
         Result := StrToFloat(PlatVerString); // Create a double from the X.Y Platform version string
      end;
   except                                                                   
      ShowExceptionMessage;
      Result:= -1.0; // Indicate in the return value that an exception was generated
   end;
end;

//
// Before the installer UI appears, verify that the required ASCOM Platform version is installed.
//
function InitializeSetup(): Boolean;
var
   PlatformVersionNumber : double;
 begin
   Result := FALSE;  // Assume failure
   PlatformVersionNumber := PlatformVersion(); // Get the installed Platform version as a double
   If PlatformVersionNumber >= REQUIRED_PLATFORM_VERSION then	// Check whether we have the minimum required Platform or newer
      Result := TRUE
   else
      if PlatformVersionNumber = 0.0 then
         MsgBox('No ASCOM Platform is installed. Please install Platform ' + Format('%3.1f', [REQUIRED_PLATFORM_VERSION]) + ' or later from https://www.ascom-standards.org', mbCriticalError, MB_OK)
      else 
         MsgBox('ASCOM Platform ' + Format('%3.1f', [REQUIRED_PLATFORM_VERSION]) + ' or later is required, but Platform '+ Format('%3.1f', [PlatformVersionNumber]) + ' is installed. Please install the latest Platform before continuing; you will find it at https://www.ascom-standards.org', mbCriticalError, MB_OK);
end;

// Code to enable the installer to uninstall previous versions of itself when a new version is installed
procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
  UninstallExe: String;
  UninstallRegistry: String;
begin
  if (CurStep = ssInstall) then // Install step has started
	begin
      // Create the correct registry location name, which is based on the AppId
      UninstallRegistry := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}' + '_is1');
      // Check whether an extry exists
      if RegQueryStringValue(HKLM, UninstallRegistry, 'UninstallString', UninstallExe) then
        begin // Entry exists and previous version is installed so run its uninstaller quietly after informing the user
          MsgBox('Setup will now remove the previous version.', mbInformation, MB_OK);
          Exec(RemoveQuotes(UninstallExe), ' /SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
          sleep(1000);    //Give enough time for the install screen to be repainted before continuing
        end
  end;
end;

