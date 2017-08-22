# WinDeviceManagerLight
Enable and disable devices on windows

This project is to show how to programatically retrieve, enable and disable devices on a Windows computer.<br>
It's a continuation of the project found here : [https://www.codeproject.com/Articles/21503/Hardware-Helper-Library-for-C](https://www.codeproject.com/Articles/21503/Hardware-Helper-Library-for-C)

<img src="/docs/devicemanager_preview.png"/>

#### Notes
Sometimes the real Device Manager on Windows appear to virtually disable the device but it is still enabled and will ask to reboot your machine to make it effective, this project has the same flaw.

Multiple techniques has been used to change a device state but this method appears to be the only one that works and is stable enough on my machine, if you have a better one, make it known.
