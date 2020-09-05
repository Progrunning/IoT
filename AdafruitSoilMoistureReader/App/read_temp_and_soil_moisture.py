import time

from board import SCL, SDA
import busio

from adafruit_seesaw.seesaw import Seesaw

i2c_bus = busio.I2C(SCL, SDA)

ss = Seesaw(i2c_bus, addr=0x36)

soilMoistureReading = ss.moisture_read()
temperatureReading = ss.get_temp()

print("{ \"soilMoisture\":" + str(soilMoistureReading) + ", \"temperature\":" + str(temperatureReading) + " }")