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

## Building the Application

To create the required database:

```
dotnet tool install --global dotnet-ef
dotnet tool restore
dotnet ef database update
```

To build (and unit test) the application either select "Build->Build Solution" from within Visual Studio or execute the following from the command line:

```
msbuild IWA.NET.sln /p:Configuration=Debug /t:Clean,Build
```

## Running the Application

You can run the application from within Visual Studio or use the provided Azure DevOps pipeline 
[azure-pipelines.yml](azure-pipelines.yml) to deploy it to an Azure Website.

### Creating an environment (.env) file

Most of the following examples need environment and user specific credentials. These are loaded from a file called `.env`
in the project root directory. This file is not created by default (and should never be stored in source control). An example
with all of the possible settings for the following scenarios is illustrated below:

```aidl
APP_URL=http://localhost:8080
SSC_URL=http://localhost:9090/ssc
SSC_USERNAME=admin
SSC_PASSWORD=admin
SSC_AUTH_TOKEN=6b16aa46-35d7-4ea6-98c1-8b780851fb37
SSC_APP_NAME=IWA.NET
SSC_APP_VER_NAME=master
SCANCENTRAL_CTRL_URL=http://localhost:9090/scancentral-ctrl
SCANCENTRAL_CTRL_TOKEN=96846342-1349-4e36-b94f-11ed96b9a1e3
SCANCENTRAL_POOL_ID=00000000-0000-0000-0000-000000000002
SCANCENTRAL_EMAIL=info@microfocus.com
SCANCENTRAL_DAST_API=http://localhost:8088/api/
NEXUS_IQ_URL=http://localhost:8090
NEXUS_IQ_AUTH=gTvvcLQ3:NDZ6bIzhFTRIyT9UtPaQaSEc0HaDsQd3ELvXnkohBGmK
NEXUS_IQ_APP_ID=IWA
FOD_API_URL=https://api.emea.fortify.com
FOD_API_KEY=89795c5f-798c-48d5-8c4a-de999692cdd4
FOD_API_SECRET=XXXXX
DisableSSLSecurity=true
```

### SAST using Fortify SCA command line

There is an example PowerShell script [fortify-sca.ps1](bin/fortify-sca.ps1) that you can use to execute static application security testing
via [Fortify SCA](https://www.microfocus.com/en-us/products/static-code-analysis-sast/overview).

```aidl
.\bin\fortify-sca.ps1
```

This script runs a "sourceanalyzer" translation and scan on the project's source code. It creates a Fortify Project Results file called `IWA.NET.fpr`
which you can open using the Fortify `auditworkbench` tool:

```aidl
auditworkbench.cmd .\IWA.fpr
```

It also creates a PDF report called `IWA.NET.pdf` and optionally
uploads the results to [Fortify Software Security Center](https://www.microfocus.com/en-us/products/software-security-assurance-sdlc/overview) (SSC).

In order to upload to SSC you will need to have entries in the `.env` similar to the following:

```aidl
SSC_URL=http://localhost:9090/ssc
SSC_AUTH_TOKEN=28145aad-c40d-426d-942b-f6d6aec9c56f
SSC_APP_NAME=IWA.NET
SSC_APP_VER_NAME=master
```

The `SSC_AUTH_TOKEN` entry should be set to the value of a 'CIToken' created in SSC _"Administration->Token Management"_.

### SAST using Fortify ScanCentral SAST

There is a PowerShell script [fortify-scancentral-sast.ps1](bin\fortify-scancentral-sast.ps1) that you can use to package
up the project and initiate a remote scan using Fortify ScanCentral SAST:

```aidl
.\bin\fortify-scancentral-sast.ps1
```

In order to use ScanCentral SAST you will need to have entries in the `.env` similar to the following:

```aidl
SSC_URL=http://localhost:9090/ssc
SSC_AUTH_TOKEN=6b16aa46-35d7-4ea6-98c1-8b780851fb37
SSC_APP_NAME=IWA.NET
SSC_APP_VER_NAME=master
SCANCENTRAL_CTRL_URL=http://localhost:9090/scancentral-ctrl
SCANCENTRAL_CTRL_TOKEN=96846342-1349-4e36-b94f-11ed96b9a1e3
SCANCENTRAL_POOL_ID=00000000-0000-0000-0000-000000000002
SCANCENTRAL_EMAIL=test@test.com
```

The `SCANCENTRAL_CTRL_TOKEN` entry should be set to the value of a 'ScanCentralCtrlToken ' created in SSC _"Administration->Token Management"_.

Once the scan has been initiated you can check its status from the SSC User Interface or using the command:

```aidl
 scancentral -url [your-controller-url] status -token [returned-token]
```

where `[returned-token]` is the value of the token displayed after the scan request has been submitted.

### Open Source Software Composition Analysis using Sonatype Nexus

There is a PowerShell script [fortify-sourceandlibscanner.ps1](bin\fortify-sourceandlibscanner.ps1) that you can use to carry out
Open Source Software Composition Analysis (using [Sonatype Nexus](https://www.sonatype.com/products/open-source-security-dependency-management) 
and upload the results to SSC:

```aidl
.\bin\fortify-sourceandlibscanner.ps1
```

In order to user Nexus IQ Server you will need to have entries in the `.env` similar to the following:

```aidl
SSC_URL=http://localhost:9090/ssc
SSC_AUTH_TOKEN=6b16aa46-35d7-4ea6-98c1-8b780851fb37
SSC_APP_NAME=IWA.NET
SSC_APP_VER_NAME=master
NEXUS_IQ_URL=http://nexus-iq-server:8080
NEXUS_IQ_AUTH=XXX:YYY
NEXUS_IQ_APP_ID=IWA
```

where `NEXUS_IQ_AUTH` is an encoded User token created in the Nexus IQ Server UI, e.g. "User Code:Passcode". 

### SAST using Fortify on Demand

To execute a [Fortify on Demand](https://www.microfocus.com/en-us/products/application-security-testing/overview) SAST scan
you need to package and upload the source code to Fortify on Demand. To package the code into a Zip file for uploading
you can use the `scancentral` command utility as following:

```aidl
scancentral package -bt msbuild -bf IWA.NET.sln --output fod.zip
```

You can then upload this manually using the Fortify on Demand UI or you can use the PowerShell script file [fortify-fod.ps1](bin/fortify-fod.ps1) 
provided to upload the file and start a Fortify on Demand static scan as follows:

```PowerShell
.\bin\fortify-fod.ps1 -ZipFile '.\fod.zip' -ApplicationName 'IWA.NET' -ReleaseName 'master' -Notes 'PowerShell initiated scan' `
    -FodApiUri 'https://api.emea.fortify.com' -FodApiKey 'FOD_ACCESS_KEY' -FodApiSecret 'FOD_SECRET_KEY'
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

There is an example PowerShell script file [fortify-webinspect.ps1](bin\fortify-webinspect.ps1) that you can run to 
execute the scan and upload the results to SSC:

```aidl
.\bin\fortify-webinspect.ps1
```

### DAST using Fortify ScanCentral DAST

To carry out a ScanCentral DAST scan you should first "run" the application using one of the steps described above.
Then you can start a scan using the provided PowerShell script [fortify-scancentral-dast.ps1](bin\fortify-scancentral-dast.ps1).
It can be invoked via the following from a PowerShell prompt:

```PowerShell
.\bin\fortify-scancentral-dast.ps1 -ApiUri 'SCANCENTRAL_DAST_API' -Username 'SSC_USERNAME' -Password 'SSC_PASSWORD' `
    -CiCdToken 'CICD_TOKEN_ID'
``` 

where `SCANCENTRAL_DAST_API` is the URL of the ScanCentral DAST API configured in SSC and
`SSC_USERNAME` and `SSC_PASSWORD` are the login credentials of a Software Security Center user who is permitted to
run scans. Finally, `CICD_TOKEN_ID` is the "CICD identifier" of the "Scan Settings" you have previously created from the UI.

### DAST using Fortify on Demand

You can invoke a Fortify on Demand dynamic scan using the [PowerShellForFOD](https://github.com/fortify-community-plugins/PowerShellForFOD) PowerShell module.
For examples on how to achieve this see [here](https://github.com/fortify-community-plugins/PowerShellForFOD/blob/master/USAGE.md#starting-a-dynamic-scan).

### API Security Testing using Fortify WebInspect and Postman

TBD

### API Security Testing using ScanCentral DAST and Postman

TBD

## Build and Pipeline Integrations

### Azure DevOps Pipelines

An Azure Devops pipeline [azure-pipelines.yml](azure-pipelines.yml) is provided and has user
selected variables such as "UseFoD" or "UseScanCentralDAST" which can be set to True or False depending
on which application security testing integration you require.

## Developing and Contributing

TBD

## Licensing

This application is made available under the [GNU General Public License V3](LICENSE)