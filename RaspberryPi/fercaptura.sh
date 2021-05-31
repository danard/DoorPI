#!/bin/bash

DATE=$(date +"%Y-%m-%d_%H%M")

fswebcam -r 1280x720 /home/pi/captures/$DATE.jpg

echo "-- sending image --"
curl -i -F 'image=@/home/RaspberryPi/captures/'$DATE'.jpg' http://nattech.fib.upc.edu:40330/uploadimg
