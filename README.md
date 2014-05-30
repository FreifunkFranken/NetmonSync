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

Parameter   | Description                      | Example
----------- | ---------------------------------| --------------------------------------
netmonURL   | URL to Netmon ending with "/"    | https://netmon.freifunk-franken.de/
libremapURL | URL to LibreMap ending with "/"  | http://95.85.40.145:5984/libremap-dev/
delay       | Time in minutes waiting          | 10

```
mono NetmonSync/bin/Debug/NetmonSync.exe %netmonURL% %libremapURL% %delay%
```
For FreifunkFranken Run this:
```
mono NetmonSync/bin/Debug/NetmonSync.exe https://netmon.freifunk-franken.de/ http://95.85.40.145:5984/libremap-dev/ 10
```
