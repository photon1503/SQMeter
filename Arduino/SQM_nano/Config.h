// Config.h 
// User specific configuration for SQM
// 
// Copyright (c) 2019 Roman Hujer   http://hujer.net
//
#ifndef CONFIG_H
#define CONFIG_H

#define SERIAL_BAUD 9600    //Serial port baud Default is 115200

// Select only one your display
#define SH1106_ON  // SH1106  1.3"  128x64 OLED display is default
#define SSD1306_OFF // SSD1306 0.96" 128x64 OLED display 

#define EXTENDET_INFO_ON  // Two extended Startup info screen
 
#define EXTENDET_PROTOCOL_OFF  // My Extender protocol for weather info (Disable when is project compilation Big)



#define SQM_CAL_OFFSET  -1.0  // default SQM Calibration offset but priority is in EEPROM stored value
#define TEMP_CAL_OFFSET 0.0   // default Temperature Calibration offset but priority is in EEPROM stored value

#endif
