import framebuf
import gc
import json
from machine import Pin, SPI
import network
import time
import urequests
import utime
import os
import machine

# Display resolution
EPD_WIDTH       = 800
EPD_HEIGHT      = 480

RST_PIN         = 12
DC_PIN          = 8
CS_PIN          = 9
BUSY_PIN        = 13

class EPD_7in5_B:
    def __init__(self):
        self.reset_pin = Pin(RST_PIN, Pin.OUT)
        
        self.busy_pin = Pin(BUSY_PIN, Pin.IN, Pin.PULL_UP)
        self.cs_pin = Pin(CS_PIN, Pin.OUT)
        self.width = EPD_WIDTH
        self.height = EPD_HEIGHT
        
        self.spi = SPI(1)
        self.spi.init(baudrate=4000_000)
        self.dc_pin = Pin(DC_PIN, Pin.OUT)
        
        self.buffer_black = bytearray(self.height * self.width // 8)
        self.buffer_red = bytearray(self.height * self.width // 8)
        self.imageblack = framebuf.FrameBuffer(self.buffer_black, self.width, self.height, framebuf.MONO_HLSB)
        self.imagered = framebuf.FrameBuffer(self.buffer_red, self.width, self.height, framebuf.MONO_HLSB)
        self.init()

    def digital_write(self, pin, value):
        pin.value(value)

    def digital_read(self, pin):
        return pin.value()

    def delay_ms(self, delaytime):
        utime.sleep(delaytime / 1000.0)

    def spi_writebyte(self, data):
        self.spi.write(bytearray(data))

    def module_exit(self):
        self.digital_write(self.reset_pin, 0)

    # Hardware reset
    def reset(self):
        self.digital_write(self.reset_pin, 1)
        self.delay_ms(200) 
        self.digital_write(self.reset_pin, 0)
        self.delay_ms(2)
        self.digital_write(self.reset_pin, 1)
        self.delay_ms(200)   

    def send_command(self, command):
        self.digital_write(self.dc_pin, 0)
        self.digital_write(self.cs_pin, 0)
        self.spi_writebyte([command])
        self.digital_write(self.cs_pin, 1)

    def send_data(self, data):
        self.digital_write(self.dc_pin, 1)
        self.digital_write(self.cs_pin, 0)
        self.spi_writebyte([data])
        self.digital_write(self.cs_pin, 1)
        
    def send_data1(self, buf):
        self.digital_write(self.dc_pin, 1)
        self.digital_write(self.cs_pin, 0)
        self.spi.write(bytearray(buf))
        self.digital_write(self.cs_pin, 1)

    def WaitUntilIdle(self):
        print("e-Paper busy")
        while(self.digital_read(self.busy_pin) == 0):   # Wait until the busy_pin goes LOW
            self.delay_ms(20)
        self.delay_ms(20) 
        print("e-Paper busy release")  

    def TurnOnDisplay(self):
        self.send_command(0x12) # DISPLAY REFRESH
        self.delay_ms(100)      #!!!The delay here is necessary, 200uS at least!!!
        self.WaitUntilIdle()
        
    def init(self):
        # EPD hardware init start     
        self.reset()
        
        self.send_command(0x06)     # btst
        self.send_data(0x17)
        self.send_data(0x17)
        self.send_data(0x28)        # If an exception is displayed, try using 0x38
        self.send_data(0x17)
        
        self.send_command(0x04)  # POWER ON
        self.delay_ms(100)
        self.WaitUntilIdle()

        self.send_command(0X00)   # PANNEL SETTING
        self.send_data(0x0F)      # KW-3f   KWR-2F	BWROTP 0f	BWOTP 1f

        self.send_command(0x61)     # tres
        self.send_data(0x03)     # source 800
        self.send_data(0x20)
        self.send_data(0x01)     # gate 480
        self.send_data(0xE0)

        self.send_command(0X15)
        self.send_data(0x00)

        self.send_command(0X50)     # VCOM AND DATA INTERVAL SETTING
        self.send_data(0x11)
        self.send_data(0x07)

        self.send_command(0X60)     # TCON SETTING
        self.send_data(0x22)

        self.send_command(0x65)     # Resolution setting
        self.send_data(0x00)
        self.send_data(0x00)     # 800*480
        self.send_data(0x00)
        self.send_data(0x00)
        
        return 0

    def Clear(self):
        
        high = self.height
        if( self.width % 8 == 0) :
            wide =  self.width // 8
        else :
            wide =  self.width // 8 + 1
        
        self.send_command(0x10)
        for i in range(0, wide):
            self.send_data1([0xff] * high)
                
        self.send_command(0x13) 
        for i in range(0, wide):
            self.send_data1([0x00] * high)
                
        self.TurnOnDisplay()
        
    def display(self):
        
        high = self.height
        if( self.width % 8 == 0) :
            wide =  self.width // 8
        else :
            wide =  self.width // 8 + 1
        
        # send black data
        self.send_command(0x10) 
        for i in range(0, wide):
            self.send_data1(self.buffer_black[(i * high) : ((i+1) * high)])
            
        # send red data
        self.send_command(0x13) 
        for i in range(0, wide):
            self.send_data1(self.buffer_red[(i * high) : ((i+1) * high)])
            
        self.TurnOnDisplay()

    def sleep(self):
        self.send_command(0x02) # power off
        self.WaitUntilIdle()
        self.send_command(0x07) # deep sleep
        self.send_data(0xa5)

def turn_led_on():
    pin = Pin("LED", Pin.OUT)
    pin.on()

def turn_led_off():
    pin = Pin("LED", Pin.OUT)
    pin.off()

def connect_wifi(config):
    print('Connecting to WiFi')
    # waiting some time between connecting and checking the status is crucial for a reliable WiFi connection
    wifi = network.WLAN(network.STA_IF)
    wifi.active(True)
    time.sleep(2)
    wifi.connect(config['ssid'], config['pw'])
    time.sleep(5)

    max_wait = 30
    while max_wait > 0:
        if wifi.status() >= 3:
            break
        max_wait -= 1
        time.sleep(2)

    if wifi.status() != 3:
        turn_led_on()
        raise RuntimeError('network connection failed')
    
    print('WiFi connection established')
    wifi = None

if __name__=='__main__':
    print('Starting')
    gc.enable()

    turn_led_off()

    sleep_in_seconds = 60
    empty_buffer = bytearray(0)

    try:
        config_file = open('config.json', 'r')
        config = json.loads(config_file.read())
        config_file.close()

        connect_wifi(config)

        gc.collect()
        print('Requesting backend')
        response = urequests.get('http://%s/screenshotCreator/latestImage?blackAndWhite=true&asWaveshareBytes=true&addWaveshareInstructions=true'%(config['host']))

        if response.status_code == 200:
            print('Response indicated success')
            update_screen = response.headers.get('waveshare-update-screen', 'True') == 'True'
            sleep_in_seconds = int(response.headers.get('waveshare-sleep-between-updates', '60'))

            if update_screen:
                print('Updating screen')
                epd = EPD_7in5_B()
                epd.Clear()

                last_modified = response.headers.get('waveshare-last-modified-local-time', 'xx:xx')

                # assign black and red portion
                gc.collect()
                time.sleep(1)
                epd.buffer_black = response.content
                epd.imagered.fill(0x00)
                epd.imagered.text(last_modified, 740, 460, 0xff)

                # display and go to deep sleep
                epd.display()
                epd.sleep()

                # do not leak memory
                epd.buffer_black = empty_buffer
                update_screen = None
                last_modified = None
                response = None
            turn_led_off()
        else:
            print('Response did not indicate success')
            turn_led_on()
    except Exception as e:
        print(e)
        error_file = open('error.txt', 'w')
        error_file.write(str(e))
        error_file.close()
        turn_led_on()

    print('Sleeping for %s s'%(sleep_in_seconds))
    network.WLAN(network.STA_IF).deinit()
    machine.freq(18000000)
    time.sleep(sleep_in_seconds)
    machine.reset()