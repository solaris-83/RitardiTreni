@echo off
set "NSIS_PATH=%ProgramFiles(x86)%\NSIS\makensis.exe"
rem set "MSBUILD_PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
set "MSBUILD_PATH=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\msbuild.exe"
rem set "SIGNTOOL_PATH=%ProgramFiles(x86)%\Microsoft SDKs\ClickOnce\SignTool\SignTool.exe"
set "SIGNTOOL_PATH=%ProgramFiles(x86)%\Windows Kits\8.1\bin\x64\SignTool.exe"

REM ###########################################################
REM #####                                     Delete old files                                       ####
REM ###########################################################
del .\Sources\*.* /Q
del .\Output\*.* /Q

REM ###########################################################
REM #####                             Build visual studio project                             ####
REM ###########################################################
"%MSBUILD_PATH%" "..\Ritardi treni.sln" /t:Build /p:Configuration=Release /p:Platform="Any CPU"

REM ###########################################################
REM #####                            Copy source files                                             ####
REM ###########################################################
xcopy "..\Ritardi treni\bin\Release\*.*" .\Sources /E /y
copy .\Prerequisites\Icons\icons8_train_480_Qz7_icon.ico .\Sources /y

REM ###########################################################
REM #####               Signing the GaugeFlex_QAS_MigrationToolkit.exe file             ####
REM ###########################################################
REM "%SIGNTOOL_PATH%" sign /debug /v /f "AtlasCopcoBLM_2019.pfx" /p "@tl@5!2017b1m" /t http://timestamp.verisign.com/scripts/timstamp.dll ".\Sources\GaugeFlex_QAS_MigrationToolkit.exe"

REM ###########################################################
REM #####                                Build NSIS stup projects                               ####
REM ###########################################################
"%NSIS_PATH%" "Utilities\ExtractVersionInfo.nsi" 
"%NSIS_PATH%" "SetupProject.nsi" 

REM ###########################################################
REM #####         Wait for setup.exe to not be locked, then sign the file     ####
REM ###########################################################

mkdir ".\Output"
%SystemRoot%\explorer.exe ".\Output"
pause