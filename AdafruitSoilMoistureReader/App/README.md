# Overview

This is a project that will use the Rasperry Pi 3a+ and the Adafruit STEMMA Soil Sensor - I2C Capacitive Moisture Sensor to measure soil moisture in plants.

# Technology

This is a .NET Core project, which executes Python's scripts under the hood. The reason for this combintaion is that I'm a .NET developer and don't know Python well enough to write it purley in it. 

The software and tooling for the soil moisture sensor is Python based and to avoid re-inventing the wheel (i.e. rewriting Python code in .NET) this project is utilising the work done by Adafruit and contributors to this project https://github.com/adafruit/Adafruit_CircuitPython_seesaw

# How To

Below are the materials, guidelines and useful commands that I've gathered while experimenting with this project.

## Docs

- Python & CirtcutPython setup for the Adafruit STEMMA Soil Moisture sensor -> https://learn.adafruit.com/adafruit-stemma-soil-sensor-i2c-capacitive-moisture-sensor/python-circuitpython-test

- IoT with .NET Core playlist of 10 videos that will walk you through setup of the Rasperry Pi and .NET Core  https://www.youtube.com/watch?v=wN-wOQ-SpsU&list=PLdo4fOcmZ0oVZN5yrJbnJ70tMe9itQ10W&index=6

## Publishing

In order to publish this project one will need to execute the following command

`dotnet publish -r linux-arm`

NOTE: Once the publish is done you will need to copy all of the files from the `publish` folder onto your Rasperry Pi (see Copy files part of this readme).

## Linux / Windows Commands

### SSH Connect

In order to connect to your Rasperry Pi with Windows machine follow this guideline https://www.raspberrypi.org/documentation/remote-access/ssh/windows10.md

`ssh <pi_user>@<ip_address_of_your_pi>` - will prompt you for the password

NOTE1: If you haven't changed anything your `<pi_user>` should be `pi`.
NOTE2: You can get the IP of your Rasperry Pi by executing the following command `hostname -I` or `ipconfig`

### Copying files (SCP)

In order to copy files to your Rasperry Pi follow these instructions https://www.raspberrypi.org/documentation/remote-access/ssh/scp.md or simply try executing this command

`scp <local_file> <pi_user>@<ip_address_of_your_pi>:<pi_folder>`

or for the entire folder use `-r` flag

`scp <local_folder> <pi_user>@<ip_address_of_your_pi>:<pi_folder>`

TIP: Before copying all of the files from the `publish` folder zip then up first and then copy accross

Windows Python path: "C:\\Users\\Mikolaj\\AppData\\Local\\Microsoft\\WindowsApps\\python3.8.exe"
Pi Python path: /usr/bin/python3 or /usr/bin/python3.7

## Run

The app takes a single input argument. The argument is the path to the Python executable. The executable for Python Version 3 is usually located in the following path `/usr/bin/python3` on your Rasperry Pi