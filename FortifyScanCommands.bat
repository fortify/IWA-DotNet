call "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat"
Del *.fpr
del *.log
sourceanalyzer -b iwa -clean
sourceanalyzer -b iwa -debug -logfile trans.log dotnet build IWA.NET.sln
sourceanalyzer -b iwa -debug -logfile trans.log InsecureWebApp\Terraform\Azure\main.tf
sourceanalyzer -b iwa -debug -logfile trans.log etc\AK8-Deploy.yml
sourceanalyzer -b iwa -debug -logfile scan.log -scan -f iwa.fpr -Dcom.fortify.sca.rules.enable_wi_correlation
start "" "iwa.fpr"
