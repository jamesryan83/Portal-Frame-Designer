Portal Frame Designer
--------------------------

C#/WPF Program that takes user inputs to describe the dimensions and member sizes of a portal frame building and outputs the data to an Excel spreadsheet where wind loads etc. are applied.

![colors image](vbalibrary_Colors.png)

It uses [Helix Toolkit](https://github.com/helix-toolkit) to draw the interactive 3D model of the portal frame and uses Excel COM Interop to take the data and run various VBA Macros which perform the calculations.  The data was then sent to Multiframe, though that code was in the spreadsheet not this program.

This program should run but you won't be able to use the "Create in Multiframe" feature as the required Excel Spreadsheet is the IP of a former employer of mine and I can't upload it here.