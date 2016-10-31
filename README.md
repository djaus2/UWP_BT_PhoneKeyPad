# UWP_BT_PhoneKeyPad
Event driven sample Universal Windows Platform app that uses some custom UWP libraries to get keypresses from a Phone Keypad that is connected to an Arduino Uno. Connectivity is over Generic Bluetooth Serial.

## About
Based upon the previous *Generic Bluetooth Serial* project, this suite of projects provides:
* An Arduino Uno event driven Sketch that captures a phone keypad events _(pressed,up and held)_ and forwards them as simple text over Bluetooth Serial
* The Generic Bluetooth Serial UWP-Arduino connectivity as a UWP class library.
* A UWP class library that connects to the Arduino device over the BT class, captures the keypad messages and generates events.
* A UWP class library that captures the keypad keypress events and actions a configured delegate for each key.
* A sample app that connects the BT class to the Arduino device, as well as implementing a delegate for each key.
