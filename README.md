#NetmonSync
Sync Netmon with LibreMap using mono

##Howto
###Install Mono
Run this command on Ubuntu:
```
sudo apt-get install mono-devel
```
###Built Software
```
xbuild NetmonSync.sln
```
The compiled program you will find in Folder **NetmonSync/bin/Debug/**.

###Run Software
This Parameter must be set on start:

Parameter    | Description                     | Example
------------ | --------------------------------| -------------------------------------------------
--netmon *   | URL to Netmon ending with "/"   | --netmon https://netmon.freifunk-franken.de/
--libremap * | URL to LibreMap ending with "/" | --libremap http://95.85.40.145:5984/libremap-dev/
--delay *    | Time in minutes waiting         | --delay 30
--log *      | Path to Logfile (optional)      | --log C:\log.txt
-d           | For DeamonMode (optional)       | -d


For FreifunkFranken Run this:
```
mono NetmonSync/bin/Debug/NetmonSync.exe --netmon https://netmon.freifunk-franken.de/ --libremap http://95.85.40.145:5984/libremap-dev/ --delay 10
```
