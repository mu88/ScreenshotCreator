from machine import I2C

class INA219:
    def __init__(self, i2c_bus=1, addr=0x40):
        self.i2c = I2C(i2c_bus)
        self.addr = addr

        # Set chip to known config values to start
        self._cal_value = 0
        self._current_lsb = 0
        self._power_lsb = 0
        self.set_calibration_32V_2A()
        
    def read(self,address):
        data = self.i2c.readfrom_mem(self.addr, address, 2)
        return ((data[0] * 256 ) + data[1])

    def write(self,address,data):
        temp = [0,0]
        temp[1] = data & 0xFF
        temp[0] =(data & 0xFF00) >> 8
        self.i2c.writeto_mem(self.addr,address,bytes(temp))

    def set_calibration_32V_2A(self):
        self._current_lsb = 1  # Current LSB = 100uA per bit
        self._cal_value = 4096
        self._power_lsb = .002  # Power LSB = 2mW per bit
        self.write(0x05,self._cal_value)

        # Set Config register to take into account the settings above
        self.bus_voltage_range = 0x01
        self.gain = 0x03
        self.bus_adc_resolution = 0x0D
        self.shunt_adc_resolution = 0x0D
        self.mode = 0x07
        self.config = self.bus_voltage_range << 13 | \
                      self.gain << 11 | \
                      self.bus_adc_resolution << 7 | \
                      self.shunt_adc_resolution << 3 | \
                      self.mode
        self.write(0x00,self.config)
        
    def getBusVoltage(self):  
        self.read(0x02)
        bus_voltage = (self.read(0x02) >> 3) * 0.004
        percentage = int((bus_voltage -3)/1.2*100)
        if(percentage<0):percentage=0
        elif(percentage>100):percentage=100

        return percentage