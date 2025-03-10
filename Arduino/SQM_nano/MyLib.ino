// MyLib.ino
// Librarry function for SQM
//
// Copyright (c) 2018 Roman Hujer   http://hujer.net
//

void _blk_change_status()
{
  if (Blik)
  {
    Blik = false;
  }
  else
  {
    Blik = true;
  }
} // end of _blk_change_status()

void buzzer(int _long)
{
  for (signed int _i = 0; _i < _long / 2; _i++)
  {
    digitalWrite(BuzzerPin, 1);
    delay(1);
    digitalWrite(BuzzerPin, 0);
    delay(1);
  }
} // end of buzzer( int _long )

void ReadWeather()
{


    pres = bme.readPressure() / 100.0F;
    temp = bme.readTemperature();
    if ( Humidity) hum =  bme.readHumidity();
     else hum = 0;
    temp = temp + TempCalOffset;  

  
}


