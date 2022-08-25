call "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat"
Del *.fpr
del *.log
sourceanalyzer -b iwa -clean
sourceanalyzer -b iwa -debug -logfile trans.log devenv IWA.NET.sln /REBUILD
sourceanalyzer -b iwa -debug -logfile scan.log -scan -f iwa.fpr -Dcom.fortify.sca.rules.enable_wi_correlation
start "" "iwa.fpr"
