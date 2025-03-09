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

  pres = 1000;
  temp = 5;
}

void mySQM()
{
  String gainString = "Max gain";
  delay(50); // display cooldown
  tsl.setGain(TSL2591_GAIN_HIGH);
  gainscale = MAXSCALE;
  luminosity = tsl.getFullLuminosity();
  delay(50);
  luminosity = tsl.getFullLuminosity(); // Read twice so value can stabilize.
  ir = luminosity >> 16;
  full = luminosity & 0xFFFF;
  visible = full - ir;
  if (visible == 0xFFFF || ir == 0xFFFF)
  {
    gainString = "High gain";
    tsl.setGain(TSL2591_GAIN_HIGH);
    gainscale = HIGHSCALE;
    luminosity = tsl.getFullLuminosity();
    delay(50);
    luminosity = tsl.getFullLuminosity();
    ir = luminosity >> 16;
    full = luminosity & 0xFFFF;
    visible = full - ir;
    if (visible == 0xFFFF || ir == 0xFFFF)
    { // look, dude. It's daylight at this point. Knock it off
      gainString = "Med gain";
      tsl.setGain(TSL2591_GAIN_MED);
      gainscale = MEDSCALE;
      luminosity = tsl.getFullLuminosity();
      delay(50);
      luminosity = tsl.getFullLuminosity();
      ir = luminosity >> 16;
      full = luminosity & 0xFFFF;
      visible = full - ir;
      if (visible == 0xFFFF || ir == 0xFFFF)
      { // ARE YOU ON THE SUN?
        gainString = "Low gain";
        tsl.setGain(TSL2591_GAIN_LOW);
        gainscale = LOWSCALE;
        luminosity = tsl.getFullLuminosity();
        delay(50);
        luminosity = tsl.getFullLuminosity();
        ir = luminosity >> 16;
        full = luminosity & 0xFFFF;
        visible = full - ir;
      }
    }
  } 
  adjustedIR = (float)ir / gainscale;
  adjustedVisible = (float)visible / gainscale;
  mag = -1.085736205 * log(.925925925 * pow(10, -5.) * adjustedVisible);
  if (isinf(mag))
  {
    mag = 25.0;
  }
  mag = mag + ReadEESqmCalOffset();
  mag = float(int(mag * 100 + 0.5)) / 100;

  lux = tsl.calculateLux(full, ir);
  // ux = (full - ir) * 0.6 / (gainscale / 428.0);
}
