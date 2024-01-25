# COPYRIGHT Atlas Copco BLM
#
# The copyright to the computer program herein is the property of
# Atlas Copco BLM, Italy. The program may be used and/or
# copied only with the written permission from Atlas Copco BLM
# or in the accordance with the terms and conditions stipulated in
# the agreement/contract under which the program has been supplied.
#
#-------------------------------------------------------------------------------
# Written By: Paolo Leggiadro
#             Atlas Copco BLM
#
# Descr: Torque Supervisor to QA Supervisor Migration Toolkit
#
# Revisions: 01
#
# 2018-09-06  Paolo Leggiadro - First release

!include "MUI.nsh" ;MUI 1.67 compatible ------
!include "LogicLib.nsh" ;Conditional logic
!include "x64.nsh" ;Windows architecture detection
!include "FileFunc.nsh" ;Operation on files (such as installation size)
!include "nsProcess.nsh" ;Kill processes
!include "WordFunc.nsh"

!system "Utilities\ExtractVersionInfo.exe"
!include "Utilities\App-Version.txt"

!define PRODUCT_NAME "Ritardi treni"
#!define PRODUCT_PUBLISHER "Marco Cigana"
#!define PRODUCT_WEB_SITE "http://www.atlascopco.com"
!define PRODUCT_DIR_REGKEY "Software\Ritardi treni"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\Ritardi treni"
!define PRODUCT_AUTORUN_KEY "Software\Microsoft\Windows\CurrentVersion\Run"
!define PRODUCT_DISPLAYNAME "Ritardi treni"
!define EXECUTABLE_FILENAME "Ritardi treni.exe"
!define NET_FRAMEWORK4_VERSION_REGKEY "Software\Microsoft\NET Framework Setup\NDP\v4\Full"
!define NET_FRAMEWORK_MINIMUM_VERSION "4.8.1"
#!define TARGET_DIR "${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
!define TARGET_DIR "${PRODUCT_NAME}"
!define NET_FRAMEWORK_SETUP_FILE "NDP481-Web.exe"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP_NOSTRETCH
!define MUI_HEADERIMAGE_BITMAP "Prerequisites\Icons\logo-qa_small2.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "Prerequisites\Icons\logo-qa_small2.bmp" 
!define MUI_HEADERIMAGE_RIGHT
; !define MUI_ICON "Prerequisites\Icons\TS_QAS.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
#!insertmacro MUI_PAGE_LICENSE "Prerequisites\License\License.rtf"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\${EXECUTABLE_FILENAME}"
!insertmacro MUI_PAGE_FINISH
; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES
; Language files
!insertmacro MUI_LANGUAGE "English"
; MUI end ------


Name "${PRODUCT_NAME}"


VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${PRODUCT_NAME}"
VIProductVersion "${Version}"
#VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${Version}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Ritardi treni"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright by Marco Cigana - 2019"


OutFile ".\Output\Ritardi treni Setup.exe"
InstallDir "$PROGRAMFILES\${TARGET_DIR}"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
LicenseForceSelection radiobuttons
ShowInstDetails hide
ShowUnInstDetails hide

RequestExecutionLevel admin

Function .onInit
	# set registry modality
  	${If} ${RunningX64}
		SetRegView 64
  	${Else}
		SetRegView 32
  	${EndIf}
	
	ClearErrors
	ReadRegStr $0 HKLM "${NET_FRAMEWORK4_VERSION_REGKEY}" "Version"
	${If} ${Errors}
		goto notFound
	${Else}
		${If} $0 == ""
			goto notFound
		${Else}
			${VersionCompare} "${NET_FRAMEWORK_MINIMUM_VERSION}" $0 $1
			${If} $1 == 1
				goto notFound
			${Else}
				goto end
			${EndIf}
		${EndIf}
	${EndIf}
	
notFound:

	MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \
		"${PRODUCT_NAME} needs the Microsoft .NET Framwork 4.8.1 to work properly. Do you want to install it now? Clicking 'NO' the installation process will abort." \
		IDYES true IDNO false		
false:
	MessageBox MB_ICONINFORMATION|MB_OK ".NET framework installation not completed. Installation aborted."
	Abort
true:
	SetOutPath "$TEMP"
	SetOverwrite on

	#Load files
	File "Prerequisites\${NET_FRAMEWORK_SETUP_FILE}"

	# Run the installer
	ExecWait '"$TEMP\${NET_FRAMEWORK_SETUP_FILE}"' $0		
	${If} $0 != '0'
		MessageBox MB_ICONINFORMATION|MB_OK ".NET framework installation not completed. Installation aborted."
		Abort
	${EndIf}

end:
	
	Call ForceNsisUninstall
FunctionEnd



;|=============================================================================|
Section "Main" SEC_MAIN
;|=============================================================================|
	SetOutPath "$INSTDIR"
	SetOverwrite on

	File /r ".\Sources\*"
	File /r ".\Prerequisites\Icons\icons8_train_480_Qz7_icon.ico"
	#File  /nonfatal /r ".\Prerequisites\Other\*.*"
	AccessControl::GrantOnFile \
	"$INSTDIR" "(BU)" "FullAccess"
SectionEnd
;|=============================================================================|


;|=============================================================================|
Section -AdditionalIcons
;|=============================================================================|
	SetShellVarContext all
	
	CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE_FILENAME}"
	
	#CreateDirectory "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
	#CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE_FILENAME}"
	#CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}\Uninstall ${PRODUCT_NAME}.lnk" "$INSTDIR\uninst.exe"
    CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
	CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE_FILENAME}"
	CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall ${PRODUCT_NAME}.lnk" "$INSTDIR\uninst.exe"
	#WriteIniStr "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}\Atlas Webpage.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"

	SetShellVarContext current
	#CreateDirectory "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
	#CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE_FILENAME}"
	#CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}\Uninstall ${PRODUCT_NAME}.lnk" "$INSTDIR\uninst.exe"
	CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
	CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\${EXECUTABLE_FILENAME}"
	CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall ${PRODUCT_NAME}.lnk" "$INSTDIR\uninst.exe"
	#WriteIniStr "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}\Atlas Webpage.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
SectionEnd
;|=============================================================================|



;|=============================================================================|
Section -Post
;|=============================================================================|

	${If} ${RunningX64}
		SetRegView 64
	${Else}
		SetRegView 32
	${EndIf}


	WriteUninstaller "$INSTDIR\uninst.exe"
	WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "InstallDir" "$INSTDIR"
	WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "SilentUninstall" "false"

	WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
	WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
	WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\icons8_train_480_Qz7_icon.ico"
	
	WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${Version}"
	#WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
	#WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
	
	# Calculate install dir size
	${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
	IntFmt $0 "0x%08X" $0
	WriteRegDWORD HKLM "${PRODUCT_UNINST_KEY}" "EstimatedSize" "$0"
SectionEnd
;|=============================================================================|



LangString DESC_Section_MAIN ${LANG_ENGLISH} "Ritardi treni"


;|=============================================================================|
Section Uninstall
;|=============================================================================|
	${If} ${RunningX64}
		SetRegView 64
	${Else}
		SetRegView 32
	${EndIf}

	# Get installation path from registry
	ReadRegStr $R1 HKLM  "${PRODUCT_DIR_REGKEY}" 'InstallDir'
	#Terminate application process
	#${nsProcess::CloseProcess} "${EXECUTABLE_FILENAME}" $R0
	
	# Remove application
	RMDIR /r $R1

	# Remove links for all users
	SetShellVarContext all
	Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
	RMDIR /r "$SMPROGRAMS\${PRODUCT_NAME}\"
	RMDIR "$SMPROGRAMS\${PRODUCT_NAME}\"

	# Remove link for current user
	SetShellVarContext current
	Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
	RMDIR /r "$SMPROGRAMS\${PRODUCT_NAME}\"
	RMDIR "$SMPROGRAMS\${PRODUCT_NAME}\"

	DeleteRegKey HKLM "${PRODUCT_UNINST_KEY}"
	DeleteRegValue HKLM "${PRODUCT_AUTORUN_KEY}" "${PRODUCT_NAME}"
	
	SetAutoClose true
SectionEnd
;|=============================================================================|


Function un.onInit

	${If} ${RunningX64}
		SetRegView 64
	${Else}
		SetRegView 32
	${EndIf}

	ReadRegStr $R1 HKLM  "${PRODUCT_DIR_REGKEY}" 'SilentUninstall'

	${If} $R1 == 'false'
		MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \
			"Do you want to completely remove $(^Name) and all of its features?" \
			IDYES true IDNO false
	${ElseIf} $R1 == ''
		MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \
			"Do you want to completely remove $(^Name) and all of its features?" \
			IDYES true IDNO false
	${Else}
		goto true
	${EndIf}

false:
	Abort
true:
	# Do Nothing

FunctionEnd


Function un.onUninstSuccess
	HideWindow

	${If} ${RunningX64}
		SetRegView 64
	${Else}
		SetRegView 32
	${EndIf}

	ReadRegStr $R2 HKLM  "${PRODUCT_DIR_REGKEY}" 'SilentUninstall'

	${If} $R2 == 'false'
		MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) has been removed from your computer."
	${ElseIf} $R2 == ''
		MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) has been removed from your computer."
	${EndIf}

FunctionEnd


# Check if older Setup is already installed
Function ForceNsisUninstall

	ReadRegStr $R0 HKLM  "${PRODUCT_UNINST_KEY}" 'UninstallString'

	${If} $R0 != ''
		MessageBox MB_ICONQUESTION|MB_YESNO \
			"${PRODUCT_NAME} is already installed on your computer. Do you want to uninstall it first?" \
			IDYES true IDNO false

false:
		MessageBox MB_ICONINFORMATION|MB_OK "Installation process canceled by the user"
		Abort

true:

		${If} ${RunningX64}
			SetRegView 64
		${Else}
			SetRegView 32
		${EndIf}
	
		WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "SilentUninstall" "true"
		nxs::Show /NOUNLOAD `$(^Name) Setup` /top `Uninstalling... Please wait...` /sub `$\r$\n$\r$\n Setup is performing the uninstall operations. Just few seconds...` /h 1 /pos 0 /can 0 /marquee 50
		# Get installation path from registry
		ReadRegStr $R1 HKLM  "${PRODUCT_DIR_REGKEY}" 'InstallDir'
		#Terminate application process
		#${nsProcess::CloseProcess} "${EXECUTABLE_FILENAME}" $R0
		#ExecWait '$R0 /SP- /SILENT _?=$R1'
		# Remove application
		RMDIR /r $R1

		# Remove links for all users
		SetShellVarContext all
		RMDIR /r "$SMPROGRAMS\${PRODUCT_NAME}\"
		RMDIR "$SMPROGRAMS\${PRODUCT_NAME}\"

		# Remove link for current user
		SetShellVarContext current
		RMDIR /r "$SMPROGRAMS\${PRODUCT_NAME}\"
		RMDIR "$SMPROGRAMS\${PRODUCT_NAME}\"

		DeleteRegKey HKLM "${PRODUCT_UNINST_KEY}"
		WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "SilentUninstall" "false"
		
		nxs::Destroy

	${EndIf}
FunctionEnd
