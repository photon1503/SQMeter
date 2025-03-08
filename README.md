# SQM
Arduino compatible Sky Quality Meter using the TSL2591
   

## References

https://github.com/gshau/SQM_TSL2591/

## Links

ASCOM driver https://www.dizzy.eu/downloads.html
Unihedron SQM Device Manager http://unihedron.com/projects/darksky/cd/


## Features

### USB Control mode use derived Unihedron serial protokol

#### Box info
* Request: ix 
* Response:  i,00000002,00000003,00000001,20191012

#### Read data  
* Request: rx  
* Response: r, 10.28m,0000000000Hz,0000000002c,000005.000s, 026.2C
or
* Request: ux  
* Response: u, 10.33m,0000000000Hz,0000000004c,000005.000s, 026.4C

#### Read config data  
* Request:  gx
* Response: g, 000.00m, 000.0C,TC:Y,A5,11,DC:0
 
#### Write SQM offset to EEPROM value range (-25m ... 25m)
Negative value: 
* Request:  zcal1-0.05x
* Response: z,1,-00.05m

Positive value:  
* Request:  zcal100.01x
* Response: z,1, 00.01m 

#### Write Temperature offset to EEPROM value range (-50°C ... 50°C)
Negative value: 
* Request:  zcal2-1.5x
* Response: z,2,-01.5C 

Positive value:  
* Request:  zcal2 00.5x
* Response: z,2, 00.5C 


#### Erase EEPROM set to default value 

* Request: zcalDx
* Response: zxdL 