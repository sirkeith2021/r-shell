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

# Project Project Rshell documentary
( This documentary has a lot information and it's talking about features that are not in this version. We are not going to publish the version which has all these features! )

17.9.2020: Today we have started documenting our project Rshell. Rshell has now been detected by many Antivirus programs, for example Windows defender. This isn't a good thing. We are thinking of starting version 2.8 that will be based on current version 2.7. 

18.9.2020: Rshell is now on version 2.8 (beta) which we moved to today. We haven't added any new features yet. Right now we have some nice custom features ready and you also can run any cmd / powershell commands through this. We have now changed all string names and modified some methods to make it more undetected. Rshell is still detected by 4/26 anti-virus companies. Seems like windows defender is no longer detecting Rshell which is good news but it's just a matter of time when it does that again. We really have to figure something out to prevent this happening!

19.9.2020: We have updated our anti virtual machine system to detect online analysis virtual machines like any.run and similar one’s. It can now detect over 7 different virtual machines! We also added some new programs to our anti analysis black list

20.9.2020: We have now sold our project and it’s source to a person who isn’t part of our project. Project Rshell will not be continued for a while. We will have to change all our methods (anti VM and anti analysis) before we continue updating Rshell. Reason for making new methods is because there’s now someone else (maybe even a group of people) using the same methods as Rshell does which will possibly make it more detectable.


21.9.2020: Rshell project is now back on. Our project is now being continued as we builded new Anti-VM system which completely evades at least these: Any.Run, Intezer Analyser, VirusTotal jujubox, Hybrid analysis, Cuckoo sandbox and Joe sandbox. It probably also evades a lot more but we have not yet tested it on other sandbox / malware services. This was a huge step on our project. We have also modified some parts of our current code to make it work faster and to make it more stealth. We think that we have succeeded in our current goal!

8.11.2020: Literally nothing was done in the last 2 months. Now we have updated our varis system, we have added lots of more applications to black-list. Today we also added the “Papukaija” system which basically does the same as variksenpelätin but only checks if the user is running vm-ware which is a virtualization system commonly used by malware researchers. In nutshell it’s just stealth anti-vmware code made using Faulty. We have also tried to add better support on running commands, some why they are running quite slowly. Trying to add some kind of proxy support to balance the bandwidth and hide C2 servers better.

9.11.2020: Adding browser hijacker. Rshell can now steal Passwords, Cookies, Bookmarks and history from browsers using gecko encryption. List of browsers: Firefox, Waterfox, Cyberfox, K-Meleon, Thunderbird, IceDragon, BlackHaw, Pale Moon. We will be adding Chrome/Chromium soon x) More command handling has been made. Password stealer now works quite much faster and without problems. 

28.11.2020: Updated our browser hijacker and added some new nice methods to hide what the malware is doing. Detections are now 1/26 and the 1 over there is false detection. Also made some cool ASCII art to the main screen. It looks great now. 

29.11.2020: Added command called “Destroy”. Well the name should tell enough right? Oh it doesn’t? Let me explain then. Basically it just tries it’s best to delete everything from the pc, sadly it doesn’t yet crypt everything first so hackers or cops could easily restore all deleted data. Will more than probably add some kind of crypting method into the deleting method. Did some small tests on our anti-vm system and yes. It works great. Making small changes to the registry of the virtual machine made the software think it’s not a virtual environment but I have now made it check everything and there’s now literally no way you could bypass my anti-vm system. I’m quite proud of my creation.

24.12.2020: huh.. It's been a while! Well all we have done now is just added the stealer feature to add data into readable text file so it's easier to read.
