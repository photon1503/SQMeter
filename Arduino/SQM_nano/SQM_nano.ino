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
#include <Adafruit_TSL2591.h>

#include <math.h>

#define LOWSCALE 1.0
#define MEDSCALE 25.0
#define HIGHSCALE 428.0
#define MAXSCALE 9876.0

#define MAX_BUFFER_SIZE 64

Adafruit_TSL2591 tsl = Adafruit_TSL2591(2591);
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

  tsl.begin();
  tsl.setGain(TSL2591_GAIN_MAX);
  tsl.setTiming(TSL2591_INTEGRATIONTIME_600MS);

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
  else if (strcmp(command, "r") == 0)
  {
    mySQM();
    sqm_string = String((mag < 0) ? -mag : mag, 2);
    while (sqm_string.length() < 5)
    {
      sqm_string = '0' + sqm_string;
    }
    _sign = (mag < 0) ? '-' : ' ';
    sqm_string = _sign + sqm_string;

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
    Serial.println("g," + sqm_string + "m," + temp_string + "C,TC:" + "N," + oled + ",DC:");

   
  }
  // advanced response

  else if (command[0] == 'a') {
    mySQM();
    Serial.print("a,");
    Serial.print(luminosity);
    Serial.print(",");
    Serial.print(ir);
    Serial.print(",");
    Serial.print(adjustedIR);
    Serial.print(",");
    Serial.print(visible);
    Serial.print(",");
    Serial.print(adjustedVisible);
    Serial.print(",");
    Serial.print(full);
    
    Serial.print(",");
    Serial.print(gainscale);
    Serial.print(",");
    char luxBuffer[16];
    dtostrf(lux, 10, 6, luxBuffer); // 7 is the minimum width, 3 is the number of decimal places


    Serial.print(luxBuffer);
    Serial.print(",");
    char magBuffer[10];
    dtostrf(mag, 7, 3, magBuffer); // 7 is the minimum width, 3 is the number of decimal places

    Serial.println(magBuffer);


 
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



      // Delete calibration (note upper case "D") - set default factory value see Setup.h
      else if (_x == 'D')
      {
        SqmCalOffset = SQM_CAL_OFFSET;   // set to default
        TempCalOffset = TEMP_CAL_OFFSET; // set to default
        WriteEETempCalOffset(TempCalOffset);
        WriteEESqmCalOffset(SqmCalOffset);
        WriteEEScontras(DEFALUT_CONTRAS);
        Serial.println("zxdL");
      }
    }

    SqmCalOffset = ReadEESqmCalOffset();   // SQM Calibration offset from EEPROM
   
  }
}

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