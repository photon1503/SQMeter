# SQMeter

Arduino-compatible Sky Quality Meter using the TSL2591

* SQM-LU serial protocol
* Ability to store calibration factors in EEPROM
* Windows program for monitoring and configuration
* Compatible with ASCOM driver for Weather Conditions


## References

https://github.com/romanhujer/SQM
https://github.com/gshau/SQM_TSL2591/

## Links

[ASCOM driver](https://www.dizzy.eu/downloads.html)


## Building

Necessary components

* Arduino Nano
* TSL2591 light sensor
* 16mm M12 lens
* M12 lens holder
* some case

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
* **Response:** `g, 000.00m, 000.0C,TC:Y,A5,11,DC:0`
 
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