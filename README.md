[![Build Status](https://dev.azure.com/mfdemouk/IWA.NET/_apis/build/status/IWA%20-%20Fortify%20-%20CI?branchName=master)](https://dev.azure.com/mfdemouk/IWA.NET/_build/latest?definitionId=5&branchName=master)

# IWA .NET (Insecure Web App) Pharmacy Direct

#### Table of Contents

*   [Overview](#overview)
*   [Forking the Repository](#forking-the-repository)
*   [Building the Application](#building-the-application)
*   [Running the Application](#running-the-application)
*   [Application Security Testing Integrations](#application-security-testing-integrations)
    * [SAST using Fortify SCA command line](#static-analysis-using-fortify-sca-command-line)
    * [SAST using Fortify ScanCentral SAST](#static-analysis-using-fortify-scancentral-sast)
    * [Open Source Susceptibility Analysis using Sonatype Nexus IQ](#open-source-susceptibility-analysis-using-sonatype-nexus-iq)
    * [SAST using Fortify on Demand](#static-analysis-using-fortify-on-demand)
    * [DAST using Fortify WebInspect](#dynamic-analysis-using-fortify-webinspect)
    * [DAST using Fortify ScanCentral DAST](#dynamic-analysis-using-fortify-scancentral-dast)
    * [DAST using Fortify on Demand](#dynamic-analysis-using-fortify-on-demand)
    * [API Security Testing using Fortify WebInspect and Postman](#api-security-testing-using-fortify-webinspect-and-postman)
    * [API Security Testing using ScanCentral DAST](#api-security-testing-using-scancentral-dast-and-postman)
*   [Build and Pipeline Integrations](#build-and-pipeline-integrations)
    * [Azure DevOps Pipeline](#azure-devops-pipelines)
*   [Developing and Contributing](#developing-and-contributing)
*   [Licensing](#licensing)

## Overview

_IWA.NET (Insecure Web App) Pharmacy Direct_ is an example Microsoft.NET Core Web Application for use in **DevSecOps** scenarios and demonstrations.
It includes some examples of bad and insecure code - which can be found using static and dynamic application
security testing tools such as [Micro Focus Fortify](https://www.microfocus.com/en-us/cyberres/application-security).

One of the main aims of this project is to illustrate how security can be embedded early ("Shift-Left") and continuously ("CI/CD") in
the development lifecycle. Therefore, a number of examples of "integrations" to common CI/CD pipeline tools are provided.

The application is intended to provide the functionality of a typical "online pharmacy", including purchasing Products (medication)
and requesting Services (prescriptions, health checks etc). It has a modern-ish HTML front end (with some JavaScript) and a Swagger based API.

*Please note: the application should not be used in a production environment!*

![Screenshot](media/screenshot.png)

## Forking the Repository

In order to execute the example scenarios described here you should "fork" a copy of the Git repository - unfortunately Azure DevOps
does not offer this feature across organisations. As an alternative you can "clone" the repository and setup a new "remote" or ask
permission to be added to the dedicated fork project.

## Pre-Requisites

 - DotNet Framework 6.0.25
 - SQL Server Express 2019 including SQL Server LocalDB
 - Visual Studio 2022 Community Edition (or higher)

## Building the Application


To create/populate the required database:

```
cd InsecureWebApp
dotnet tool install --global dotnet-ef --version 6.0
dotnet tool restore
dotnet ef database update
```

To build (and unit test) the application either select "Build->Build Solution" from within Visual Studio or execute the following from a
Visual Studio Developers command prompt from the root directory of the project:

```
dotnet restore
msbuild IWA.NET.sln /p:Configuration=Debug /t:Clean,Build
```
## Docker Build

To create docker image:

```
docker build --tag iwa.net --file InsecureWebApp/Dockerfile .
```
To create and test locally, no support of SSL, use httpport=44331 to test i.e., http://localhost:44331/:

```
docker run -d -p 44331:80 iwa.net
```


## Running the Application

You can run the application from within Visual Studio or use the provided Azure DevOps pipeline 
[azure-pipelines.yml](azure-pipelines.yml) to deploy it to an Azure Website.

### SAST using Fortify SCA command line

There is an example batch script [fortify-sca.ps1](bin/fortify-sca.ps1) that you can use to execute static application security testing
via [Fortify SCA](https://www.microfocus.com/en-us/products/static-code-analysis-sast/overview).

```aidl
FortifyScanCommands.bat
```
OR directly run the below commands to execute SAST

```aidl
sourceanalyzer -b iwa -clean
sourceanalyzer -b iwa -debug -logfile trans.log dotnet build IWA.NET.sln
sourceanalyzer -b iwa -debug -logfile scan.log -scan -f iwa.fpr
start "" "iwa.fpr"
```

This script runs a "sourceanalyzer" translation and scan on the project's source code. It creates a Fortify Project Results file called `IWA.fpr`
which you can open using the Fortify `auditworkbench` tool:

```aidl
auditworkbench IWA.fpr
```

### SAST using Fortify on Demand

To execute a [Fortify on Demand](https://www.microfocus.com/en-us/products/application-security-testing/overview) SAST scan
you need to package and upload the source code to Fortify on Demand. To package the code into a Zip file for uploading
you can use the `scancentral` command utility as following:

```aidl
"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat"
scancentral package --build-tool msbuild --build-file IWA.NET.sln --output fod.zip
```

You can then upload this manually using the Fortify on Demand UI or you can use the FoDUploader utility to upload the file 
and start a Fortify on Demand static scan as follows:

```Cmd
java -jar FodUpload.jar -z fod.zip -aurl 'https://api.emea.fortify.com' -purl 'https://emea.fortify.com' -rid 'FOD_RELEASE_ID' -tc 'FOD_TENANT'
	-uc 'FOD_CREDENTIALS' -ep 2 -pp 0 -I 1 -apf -n "command-line initiated scan"
``` 

where `FOD_ACCESS_KEY` and `FOD_SECRET_KEY` are the values of an API Key and Secret you have created in the Fortify on
Demand portal. This script makes use of the [PowerShellForFOD](https://github.com/fortify-community-plugins/PowerShellForFOD) 
PowerShell module.

### DAST using Fortify WebInspect

To carry out a WebInspect scan you should first "run" the application using one of the steps described above.
Then you can start a scan using the following command line:

```
"C:\Program Files\Fortify\Fortify WebInspect\WI.exe" -s ".\etc\IWA-UI-Dev-Settings.xml" -macro ".\etc\IWA-UI-Dev-Login.webmacro" -u "http://localhost:8080" -ep ".\IWA.NET-DAST.fpr" -ps 1008
```

This will start a scan using the Default Settings and Login Macro files provided in the `etc` directory. It assumes
the application is running on "localhost:8080". It will run a "Critical and High Priority" scan using the policy with id 1008. 
Once completed you can open the WebInspect "Desktop Client" and navigate to the scan created for this execution. An FPR file
called `IWA.NET-DAST.fpr` will also be available - you can open it with `auditworkbench` (or generate a
PDF report from using `ReportGenerator`). You could also upload it to Fortify SSC or Fortify on Demand.


## Build and Pipeline Integrations

### Azure DevOps Pipelines

An Azure Devops pipeline [azure-pipelines.yml](azure-pipelines.yml) is provided and has user
selected variables such as "UseFoD" or "UseScanCentralDAST" which can be set to True or False depending
on which application security testing integration you require.

## Licensing

This application is made available under the [GNU General Public License V3](LICENSE)