/*
   SQM.ino   Sky Quality Meter

   Copyright (c) 2019 Roman Hujer   http://hujer.net

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,ss
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.

  Description:
  Sky Quality Meter using the TSL2591

  base on https://github.com/gshau/SQM_TSL2591/

  and BME280 weather sensor

  and 128x64 OLED I2C display 0.96" (SSD1306) or 1.3" (SH1106)

  Wiring diagram a PCB  on   h
ttps://easyeda.com/hujer.roman/sqm-hr

*/
#define Version "1.0.9"
#define SERIAL_NUMBER "20200210"
#include "Config.h"
#include "Setup.h"
#include "Validate.h"

#include <Wire.h>
#include <EEPROM.h>

#include <Adafruit_Sensor.h>
// #include <Adafruit_TSL2591.h>
#include "SQM_TSL2591.h"
#include <Adafruit_BME280.h>

#include <math.h>

#define LOWSCALE 1.0
#define MEDSCALE 25.0
#define HIGHSCALE 428.0
#define MAXSCALE 9876.0

#define MAX_BUFFER_SIZE 64

// Adafruit_TSL2591 tsl = Adafruit_TSL2591(2591);
SQM_TSL2591 sqm = SQM_TSL2591(2591);
double gainscale = MAXSCALE;
uint32_t luminosity;
uint16_t ir, full, visible;
double adjustedVisible, adjustedIR;
double mag;
float lux;
float ulux;

float SqmCalOffset = SQM_CAL_OFFSET;   // SQM Calibration offset from EEPROM
float TempCalOffset = TEMP_CAL_OFFSET; // Temperature Calibration offset from EEPROM

boolean InitError = false;
boolean Blik = false;
boolean Humidity = true;

float temp = 0;
float hum = 0;
float pres = 0;
uint16_t counter = 0;
byte page = 0;
String BME_Msg;
String TSL_Msg;
char oled[6] = "A5,11";

void setup()
{

  Wire.begin();
  pinMode(ModePin, INPUT_PULLUP);

  Serial.begin(SERIAL_BAUD);
  // Serial.setTimeout(5000);
  Serial.println("Ready");

  setup_temperature();

  void readSQM(void);
  if (sqm.begin())
  {
    sqm.verbose = false; // debug
    sqm.config.gain = TSL2591_GAIN_MAX;
    sqm.config.time = TSL2591_INTEGRATIONTIME_600MS;
    sqm.configSensor();
  }
  else
  {
    InitError = true;
    Serial.println("TSL2591 not found");
  }

  SqmCalOffset = ReadEESqmCalOffset();   // SQM Calibration offset from EEPROM
  TempCalOffset = ReadEETempCalOffset(); // Temperature Calibration offset from EEPROM

  sqm.setCalibrationOffset(SqmCalOffset); // call offset
  float DF = ReadEEDFCal();
  sqm.setDF(DF);               // DF

} // end of Setup

//=======================================================================================

void processCommand(const char *command)
{
  // Format the counter string with leading zeros
  char counter_buffer[11];
  sprintf(counter_buffer, "%010u", counter++);
  String counter_string = String(counter_buffer);

  // String response;
  char response[4];
  String sqm_string;
  String temp_string;

  char _sign;

  // Unit information request (note lower case "i")
  if (strcmp(command, "i") == 0)
  {
    Serial.print("i,00000002,00000003,00000001,");
    Serial.println(SERIAL_NUMBER);

    // Reading request
  }
  else if (strcmp(command, "q") == 0)
  {
    //reboot
    asm volatile("  jmp 0");
  }
  else if (strcmp(command, "v") == 0)
  {
    sqm.verbose = true;
    Serial.println("v,1");
  }
  else if (strcmp(command, "y")== 0) {
    sqm.verbose = false;
    Serial.println("y,0");
  }
  else if (strcmp(command, "r") == 0)
  {
    ReadWeather();
    if (ReadEEAutoTempCal())
      sqm.setTemperature(temp); // temp call
    // mySQM();
    sqm.takeReading();

    float mag = sqm.mpsas + ReadEESqmCalOffset();

    sqm_string = String((mag < 0) ? -mag : mag, 2);
    while (sqm_string.length() < 5)
    {
      sqm_string = '0' + sqm_string;
    }
    _sign = (mag < 0) ? '-' : ' ';
    sqm_string = _sign + sqm_string;

    temp = get_temperature();
    temp_string = String((temp < 0) ? -temp : temp, 1);
    while (temp_string.length() < 5)
    {
      temp_string = '0' + temp_string;
    }
    _sign = (temp < 0) ? '-' : ' ';
    temp_string = _sign + temp_string;
    Serial.println("r," + sqm_string + "m,0000000000Hz," + counter_string + "c,0000000.000s," + temp_string + 'C');
  }
  else if (strcmp(command, "u") == 0)
  {
    Serial.println("u," + sqm_string + "m,0000000000Hz," + counter_string + "c,0000000.000s," + temp_string + 'C');
  }
  else if (strcmp(command, "g") == 0)
  {
    sqm_string = String((SqmCalOffset < 0) ? -SqmCalOffset : SqmCalOffset, 2);
    while (sqm_string.length() < 6)
    {
      sqm_string = '0' + sqm_string;
    }
    _sign = (SqmCalOffset < 0) ? '-' : ' ';
    sqm_string = _sign + sqm_string;
    temp_string = String((TempCalOffset < 0) ? -TempCalOffset : TempCalOffset, 1);
    while (temp_string.length() < 5)
    {
      temp_string = '0' + temp_string;
    }
    _sign = (TempCalOffset < 0) ? '-' : ' ';
    temp_string = _sign + temp_string;
    oled[4] = '0';
    String autocal = ReadEEAutoTempCal() ? "Y" : "N";
    Serial.println("g," + sqm_string + "m," + temp_string + "C," + autocal + "," +  sqm.getDF() );
   }
  // advanced response

  else if (command[0] == 'a')
  {
    ReadWeather();
    if (ReadEEAutoTempCal())
      sqm.setTemperature(temp); // temp call

    sqm.takeReading();

    Serial.print("a,");
    // Serial.print(luminosity);
    Serial.print(",full:");
    Serial.print(sqm.full);
    Serial.print(",ir:");
    Serial.print(sqm.ir);
    Serial.print(",vis:");
    Serial.print(sqm.vis);

    Serial.print(",mag:");
    float mag = sqm.mpsas + ReadEESqmCalOffset();
    Serial.print(mag , 3);
    Serial.print(",dmpsas:");
    Serial.print(sqm.dmpsas);
    Serial.print(",integration:");
    Serial.print(sqm.integrationValue);
    Serial.print(",gain:");
    Serial.print(sqm.gainValue);
    Serial.print(",niter:");
    Serial.print(sqm.niter);
    Serial.print(",lux:");
    Serial.print(sqm.lux, 6);
    Serial.print(",temp:");
    Serial.print(temp);
    Serial.print(",hum:");
    Serial.print(hum);
    Serial.print(",pres:");
    Serial.print(pres);
    Serial.println();
  }
  else if (command[0] == 'z')
  {
    strncpy(response, command + 1, 3);
    response[3] = '\0';

    if (strcmp(response, "cal") == 0)
    {
      char _x = command[4];
      if (_x == '1')
      { // Calibration Light offest 1
        char responseBuffer[16];
        strncpy(responseBuffer, command + 5, sizeof(responseBuffer) - 1);
        responseBuffer[sizeof(responseBuffer) - 1] = '\0';
        SqmCalOffset = atof(responseBuffer);
        WriteEESqmCalOffset(SqmCalOffset);
        snprintf(responseBuffer, sizeof(responseBuffer), "%05.2f", SqmCalOffset);
        Serial.print("z,1,");
        Serial.print((SqmCalOffset < 0) ? '-' : ' ');
        Serial.println(responseBuffer);
      }
      // Calibration Temperature
      else if (_x == '2')
      {

        char responseBuffer[16];
        strncpy(responseBuffer, command + 5, sizeof(responseBuffer) - 1);
        responseBuffer[sizeof(responseBuffer) - 1] = '\0';
        TempCalOffset = atof(responseBuffer);
        WriteEETempCalOffset(TempCalOffset);
               snprintf(responseBuffer, sizeof(responseBuffer), "%05.2f", TempCalOffset);
        Serial.print("z,2,");
        Serial.print((TempCalOffset < 0) ? '-' : ' ');
        Serial.println(responseBuffer);
      }
      else if (_x == '3')  //DF
      {
        char responseBuffer[16];
        strncpy(responseBuffer, command + 5, sizeof(responseBuffer) - 1);
        responseBuffer[sizeof(responseBuffer) - 1] = '\0';
        float dfValue = atof(responseBuffer); // Convert to float
        WriteEEDFCal(dfValue);
        Serial.print("z,3,");
        Serial.println(dfValue,6);
      }
      

      // Enable temperature callibration
      else if (_x == 'e')
      {
        WriteEEAutoTempCal(true);
        Serial.println("zeaL");
      }

      // Disable temperature callibration (note lower case "d")
      else if (_x == 'd')
      {
        WriteEEAutoTempCal(false);
        //  sqm.resetTemperature();
        Serial.println("zdaL");
      }
      // Delete calibration (note upper case "D") - set default factory value see Setup.h
      else if (_x == 'D')
      {
        SqmCalOffset = SQM_CAL_OFFSET;   // set to default
        TempCalOffset = TEMP_CAL_OFFSET; // set to default
        WriteEETempCalOffset(TempCalOffset);
        WriteEESqmCalOffset(SqmCalOffset);
       
        Serial.println("zxdL");
      }
    }

    SqmCalOffset = ReadEESqmCalOffset(); // SQM Calibration offset from EEPROM
    TempCalOffset = ReadEETempCalOffset(); // Temperature Calibration offset from EEPROM
  }
}

/*
BME280 sensor
*/
Adafruit_BME280 bme; // I2C

void setup_temperature()
{
  bool status;
  // default settings
  status = bme.begin(0x76);

  if (!status)
  {
    Serial.println("Could not find a valid BME280 sensor, check wiring!");
    while (1)
      ;
  }
}
float get_temperature() { return bme.readTemperature(); }

const byte BUFFER_SIZE = 64;
char inputBuffer[BUFFER_SIZE];
byte index = 0;

void loop()
{
  while (Serial.available())
  {
    char received = Serial.read();

    if (received == '\r' || received == '\n')
    {
      continue;
    }

    if (received == 'x')
    {                              // End of command
      inputBuffer[index] = '\0';   // Null-terminate
      processCommand(inputBuffer); // Pass buffer to function
      index = 0;                   // Reset buffer
    }
    else if (index < BUFFER_SIZE - 1)
    {
      inputBuffer[index++] = received;
    }
    else
    {
      // Buffer overflow, reset index
      index = 0;
    }
  }
} // end of loop