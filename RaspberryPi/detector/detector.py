#!/usr/bin/env python

from time import sleep
import os
import RPi.GPIO as GPIO
pin = 4;

GPIO.setmode(GPIO.BCM);
GPIO.setup(pin,GPIO.IN);
estado = 0;

while True:
	if(GPIO.input(pin) == True):
		estado = 0;
		print('idle')
	else:
		if(estado == 0):
			os.system('/home/RaspberryPi/fercaptura.sh')
		estado = 1
	sleep(1);
