# SQMeter

Arduino-compatible Sky Quality Meter using the TSL2591 and BME280 sensor

* Ability to store calibration factors for SQM and Temperature in EEPROM
* Temperature compensation for the TSL2591

* Windows standalone program for monitoring and configuration

* SQM-LU serial protocol, compatible to standard ASCOM driver for Sky Quality Meter and Temperature

* Extended protocal with additional data
* ASCOM Platform 7 driver for Weather Conditions

- Sky Quality 
- Sky Brightness
- Temperature
- Humidity
- Pressure
- Dew Point (calculated)

- Option to configure rolling average of the measurements for a given time period

- ASCOM Conform Test passed


## References

https://github.com/romanhujer/SQM
https://github.com/gshau/SQM_TSL2591/

## Links

[ASCOM driver for SQM-LU](https://www.dizzy.eu/downloads.html)


## Building

Necessary components

* Arduino Nano
* TSL2591 light sensor
* BME280 temperature sensor
* 3.5mm M12 lens
* M12 lens holder
* 3D printed case

## Features

### USB Control mode use derived Unihedron serial protokol

#### Box Info
* **Request:** `ix`
* **Response:** `i,00000002,00000003,00000001,20191012`

#### Read Data  
* **Request:** `rx`  
* **Response:** `r, 10.28m,0000000000Hz,0000000002c,000005.000s, 026.2C`
* **Request:** `ux`  
* **Response:** `u, 10.33m,0000000000Hz,0000000004c,000005.000s, 026.4C`

#### Read Config Data  
* **Request:** `gx`
* **Response:** `g, 000.00m, 000.0C,TC:Y`
 
#### Write SQM Offset to EEPROM (value range: -25m to 25m)
* **Negative value:**  
  * **Request:** `zcal1-0.05x`
  * **Response:** `z,1,-00.05m`
* **Positive value:**  
  * **Request:** `zcal100.01x`
  * **Response:** `z,1, 00.01m`

#### Erase EEPROM and set to default value
* **Request:** `zcalDx`
* **Response:** `zxdL`

### Write Temperature offset to EEPROM value range (-50°C ... 50°C)
Negative value: 
* Request:  zcal2-1.5x
* Response: z,2,-01.5C 

Positive value:  
* Request:  zcal2 00.5x
* Response: z,2, 00.5C 

### Enabel SQM  temperature calibration 

* Request: zcalex
* Response: zxeaL 

#### Disable SQM  temperature calibration   (note lower case "d")

* Request:  zcaldx
* Response: zxdaL 

### Enable verbose mode

* Request: vx

#### Disable verbose mode

* Request: yx