# rshell
Good remote shell in C# .net, blocks all known debuggers and has a lot commands.

<img src="https://img0.imguh.com/2020/11/29/Screenshot_15d84b6f241cff859.png" alt="Screenshot 1" border="0">

# Some stats
Lines of code (stub): 1 679
Lines of code (listener):  768
Total: 2 447 lines of code.
Stats last updated: 9.11.2020 (Out dated as fuck) 

# Features
Updated: 24.12.2020
Not all commands are listed here!
* Prompt to start application with administrator privileges
* Anti-analysis
* Run any application on pc
* Send any Powershell or CMD commands to user
* See active windows
* Steals all saved passwords from many browsers
* see pc information
* run <program> [<arguments> <working dir>]

  # How to use?
  Requirements: some knowledge is always good + visual studio
  
  First of all you need to decide which version of rshell listener you're using, netcore version works with Linux and other one works on Windows.
  Open rshell client, find host IP which is encoded with base64, now you should add your own IP + port you will be listening at into it as base64 for example 
  http://127.0.0.1:1337/ = aHR0cDovLzEyNy4wLjAuMToxMzM3Lw==
  Now build the file and hope it works x)
