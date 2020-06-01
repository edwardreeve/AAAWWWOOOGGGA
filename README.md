# Edward-Reeve-SP

### Quiz Manager Application

To run;

Download this zip file. Extract it somewhere you can find easily, such as your desktop.

The application was built using .NET Core, and as such should work on Mac OS, Windows and Linux. It requires the .NET Core 3.1 runtime, which you can download here if you don't already have it installed;

[Download .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

1. Open a command prompt in the unzipped folder. 

On **Windows**, if you're unsure how to do this, you should be able to navigate to the folder using Folder Explorer, then hold down `Shift` and right-click anywhere inside. From the context menu that appears, select 'Open Powershell window here'

![ContextMenu](https://github.com/makersacademy/Edward-Reeve-SP/blob/master/WikiImages/Readme.png)

The following article has more detail, along with instructions for doing the same thing with a Terminal window on **Mac** - [Opening Command Prompts in specific folders](https://www.groovypost.com/howto/open-command-window-terminal-window-specific-folder-windows-mac-linux/)

2. Enter the following commands, waiting for each to complete before running the next;  
`dotnet tool install --global dotnet-ef` hit enter  
`dotnet ef database update` hit enter  
`dotnet run` hit enter  

The program should have launched. Built as a web application, it will instead be running locally, hosted by your machine. You can view the website at the following addresses;

https://localhost:5001  
http://localhost:5000

You may see warnings about trying to visit an unsafe site - these can be ignored using your browser's in-built options.

3. To close the application, terminate the running process in your command window by pressing `Ctrl+C`

4. To run the program again during the same session, you'll only need to enter the following command;     
`dotnet run` hit enter    
