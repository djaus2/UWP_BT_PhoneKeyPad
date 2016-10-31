# UWP_BT_PhoneKeyPad
Event driven sample Universal Windows Platform app that uses some custom UWP libraries to get keypresses from a Phone Keypad that is connected to an Arduino Uno. Connectivity is over Generic Bluetooth Serial.

## About
Based upon the previous *Generic Bluetooth Serial* project, this suite of projects provides:
* An Arduino Uno event driven Sketch that captures a phone keypad events _(pressed,up and held)_ and forwards them as simple text over Bluetooth Serial
* The Generic Bluetooth Serial UWP-Arduino connectivity as a UWP class library.
* A UWP class library that connects to the Arduino device over the BT class, captures the keypad messages and generates events.
* A UWP class library that captures the keypad keypress events and actions a configured delegate for each key.
* *_A sample app that connects the BT class to the Arduino device, as well as implementing a delegate for each key._*

### Arduino Bluetooth Keypad Utilities
These projects are part of a larger suite of projects on [Codeplex: KeypArd](https://keypard.codeplex.com/) and are discussed in greater detail on Embedded101.com:

[UWP and Arduino Keypad Library on Codeplex](http://embedded101.com/Blogs/David-Jones/entryid/760/Arduino-Bluetooth-Keypad-Utilities-Bluetooth-Echo-Sketch) Arduino sketches and UWP class libraries and apps, focused upon scanning a phone keypad as input (Bluetooth serial) to a UWP app running on a phone, desktop or IoT-Core device.

[UWP BluetoothSerialLib](http://embedded101.com/Blogs/David-Jones/entryid/763/Arduino-Bluetooth-Keypad-Utilities-UWP-BluetoothSerialLib) A modularisation of the Generic Bluetooth Serial App extracting out all of the UI functionality. Received serial messages are interpreted as Phone Keypad events. Depends upon KeypadUWPLib as it fires the keypad events based upon received messages.

[Bluetooth Echo Sketch](http://embedded101.com/Blogs/David-Jones/entryid/760/Arduino-Bluetooth-Keypad-Utilities-Bluetooth-Echo-Sketch)  This Sketch is a slight modification of that published as a companion to the original General Bluetooth Serial App.

[Event Keypad Sketch](http://embedded101.com/Blogs/David-Jones/entryid/761/Arduino-Bluetooth-Keypad-Utilities-Event-Keypad-Sketch) This Sketch scans a phone keypad using the Keypad library and detects key pressed, released and held events. The events generate a string that is sent serially denoting the event and key.

[Soft Keypad Test UWP App](http://embedded101.com/Blogs/David-Jones/entryid/764/Arduino-Bluetooth-Keypad-Utilities-Soft-Keypad-Test-UWP-App) A simple app to test the KeypadUWPLib. Implements a keypad as part of the app, which when pressed actions the keypad events. Some limitations imposed by lack of KeyUp event with XAML Buttons. [Keypad Events UWP Library]http://embedded101.com/Blogs/David-Jones/entryid/762/Arduino-Bluetooth-Keypad-Utilities-Keypad-Events-UWP-Library() An event driven Phone Keypad library. Includes KeyDown, KeyUp and KeyHoldling events to which event handlers can be attached in apps that use this library. Also includes a mechanism to fire those events from keypad scanning software. No hardware specific code though. These events mirror those events in the previous blog for the Arduino Keypad Event Scanning Sketch.  The next blog hooks the two up.[KeyPadKeysUWPLib](http://embedded101.com/Blogs/David-Jones/entryid/765/Arduino-Bluetooth-Keypad-Utilities-KeyPadKeysUWPLib)Abstracts the Keypressed event further by interpreting it in terms of the actual key press. Has a delegate for each key which can be programmatically set by the dependant app as the function (implemented in the app) to call when each each key is pressed. Concepotually, sits on top of KeypadUWPLib. 
