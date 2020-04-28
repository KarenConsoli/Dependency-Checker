# Dependency Checker #

## Introduction ##

When migrating software from one environment to another, many times we encounter the problem of having missing dependencies for the correct operation of the application in the new environment.

The objective of this exercise is to make an API Rest that allows us to find out if an X script has dependencies that need to be installed for its operation.

### Input ###

* Access to an S3 bucket will be provided on AWS. That bucket will contain the code for an application. Each file can have none, one, or more dependencies.

### Output ###

The code of a Rest API with the following functionalities:

* When sending the name of a file, the dependencies that this file has (if it has one) should be received as a response. If the file name is not registered within the application database, it must also be specified.

* Send the name of a library / module and receive in response the number of files registered in the database that have that dependency. After the quantity, also show a list of names.

* A container for testing the API operation locally.

### Definitions ###

* All dependencies within a project in C / C ++ will be given in the format #include <name> or #include "./path/file/file". To simplify the exercise, we are only interested in dependencies of the first type.

* "Database" can be a file that records names and values, like a dictionary. Or a fully functional database engine (MySQL, MariaDB, etc.). The decision is up to the programmer.

### Examples ###

* By sending the text “random.cpp” to an API endpoint (to search for dependencies), I expect to receive in response “boost / random / random_device.hpp, cassert, cstdlib, limits, random” (can use ',' as separator or another character, the important thing is to capture the dependencies)

* If I send the text "randem.cpp" to the same endpoint, I hope to receive in response that this file is not registered.

* If I send the text "gui.cpp" to the same endpoint, I hope to receive in response that this file has no registered dependencies. (Remember that dependencies like #include "gui / gui.hpp" we are not counting them, only those that are between <>)

* By sending the text "stddef.h" to another API endpoint (to search for files containing the dependency), I expect to receive "lmem.cpp, lmem.h, ldump.cpp, lfunc.cpp, lopcodes.cpp, lauxlib.h, llimits.h, lstate.cpp, ltablib.cpp, ldebug.cpp, lua.h, luaconf.h, lstrlib.cpp ”

* Similar to the previous point, if I send as text a non-existent dependency in the code, I hope that the application will inform me.

### Clarifications ###

* Use the desired language / framework

* The script must be able to run in one or more containers to be able to test it locally.

* The solution must have a complexity according to the problem (avoid over engineering)

* It is not necessary to deliver the finished exercise, to carry out as far as possible.

* Delivery date: Tuesday, April 28.

* Deliver a link to a repository with the application code and the containers.

## Getting Started ##

### Software Requirements ###

* [SQL Server Express 2017 or Above](https://www.microsoft.com/es-es/download/details.aspx?id=55994)

* [Docker Desktop for Windows](https://hub.docker.com/editions/community/docker-ce-desktop-windows)

* [Visual Studio 2017 Community or Above](https://visualstudio.microsoft.com/es/thank-you-downloading-visual-studio/?sku=Community&rel=16)

### Previous Requirements ###

#### SQL Server Seeding ####

* Create a Database called DependencyChecker
* Execute the DependencyCheckerSchema.script.sql (located in *"./DependencyCheckerApi/DependencyCheckerDatabase"*) in the created Database
* Locate the Seeding Script DependencyCheckerSeeding.py in *"./DependencyCheckerApi/DependencyCheckerSeeding"*. 
* Execute the Script for the First Time. 

```
CD [./DependencyCheckerApi/DependencyCheckerSeeding folder]

python DependencyCheckerSeeding.py
``` 

* The result is the connectionstring.json file located in *"./DependencyCheckerApi/DependencyCheckerSeeding"*. In this file you should write your connection credentials.

``` 
{"server": "localhost", "database": "DependencyChecker", "user": "root", "password": "root"}

``` 
* Execute the Seeding Script Again with the next command:

```
CD [./DependencyCheckerApi/DependencyCheckerSeeding folder]

python DependencyCheckerSeeding.py --folder [./DependencyCheckerApi/DependencyCheckerSeeding/src folder] --ext cpp,hpp,h,c
``` 
Seed Example:

```
python DependencyCheckerSeeding.py --folder C:\Proyectos\DependencyChecker\src --ext cpp,hpp,h,c
``` 
* The script will finish with your database seeded.

#### SQL Server Port Configuration ####

The Sql Server will run on the machine locally, not inside the containers, therefore it is necessary to open its tcp port so that it can be visible by them.

* Create an Inbound and Outbound TCP 1433 Port rule in yout firewall to allow all connections.
* Open your SQL Server Configuration Manager.
* Go to *"SQL Server Configuration Manger (local)/SQL Server Network Configuration/Protocols for SQLEXPRESS"* and double click "TCP/IP" option.
* In the tab "Protocol" go to *"General/Enabled"* and turned to True.
* In the tab "IP Addresses" go to *"IPALL/TCP Port"* (it is at the bottom) and write 1433 and go to *"IPALL/TCP Dynamic Ports"* and delete all the data.
* Restart SQL SERVER Service

#### SQL Server API Configuration ####

* Go to *"./DependencyCheckerApi/DependencyCheckerAPI"* and open appsettings.json. 
* Change your User and Password Credential, but you DONT HAVE to change "ServerName".
```
  "SQLServer": {
    "ServerName": "host.docker.internal,1433",
    "User": "sa",
    "Password": "jcM2nTuBKfrM8ffAkn"
  }
  ```

### Starting the Dependency Checker Container Generator ###

This application allows you to create as many containers as you want by lifting the web api in each of them.

* Double Click in *"./DependencyCheckerApi/DependencyCheckerContainerGenerator.exe"*. 




Development by [Karen Brenda Consoli](http://www.kbcon.com.ar)