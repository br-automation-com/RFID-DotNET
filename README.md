## Table of Contents
* [Introduction](#Introduction)
* [Requirements](#Requirements)
* [Revision History](#Revision-History)

<a name="Introduction"></a>
## Introduction
This is a sample project for using the B&R RFID reader with Visual Studio. The application starts as system try icon and reads the RFID data and send the information as key strokes. This can be used to enter the RFID data as password in any Windows application. The reader will be detected as serial device by scanning the serial COM ports.

![](/screenshot_1.png)

The application can be configured with the INI file that is located in the directory 'C:\Program Files (x86)\BrAutomation\B&R RFID Reader Setup'

| Section | Parameter | Default | Description |
|---|---|---|---|
| CHK | AutoConnect  | TRUE | Automatically search and connect to the RFID reader on start up.  |
| CHK | SendBallon  | TRUE | Show a ballon text with the RFID data when a new key is detected.  |
| CHK | SendKeys  | TRUE | Send the RFID key data as key strokes.  |
| CHK | SendKeysEnter  | TRUE | Finish the key stroke information with an enter key. |
| RFID | KeyMinLength  | 15 | Minimum length of the key. All keys shorter than this will be considered as false reading. |
| RFID | RefreshTimer  | 500 (ms) | The RFID data is update with the refresh timer. |
| RFID | ResponseTimeout  | 200 (ms) | The reader must respond within this time. |

<a name="Requirements"></a>
## Requirements
* B&R RFID reader 5E9000.29
* B&R RFID reader 5E9010.29

<a name="Revision-History"></a>
## Revision History

#### Version 0.1
- First public release
