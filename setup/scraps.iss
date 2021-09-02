#include "buttons.iss"

#define MyAppName "Scraps"
#define MyAppVersion "4.2.5.0"
#define MyAppPublisher "Caprine Logic"
#define MyAppExeName "Scraps.GUI.exe"

[Setup]
AppId={{D3BD46E5-E1AF-41DA-92A3-4443B418294C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL=https://github.com/depthbomb
AppSupportURL=https://github.com/depthbomb/Scraps
AppUpdatesURL=https://github.com/depthbomb/Scraps/releases
AppCopyright=Copyright (C) 2021 Caprine Logic
VersionInfoVersion={#MyAppVersion}
DefaultDirName={autopf}\{#MyAppPublisher}\{#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
AllowNoIcons=yes
LicenseFile=.\license.txt
OutputDir=..\Scraps\bin\Publish
OutputBaseFilename=scraps_setup
SetupIconFile=..\Scraps\Scraps.ico
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=classic
WizardImageFile=.\images\Image_*.bmp
WizardSmallImageFile=.\images\SmallImage_*.bmp
ArchitecturesAllowed=x64
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName=Scraps - Scrap.TF Raffle Bot

[Code]
function FromUpdate: Boolean;
begin
	Result := ExpandConstant('{param:update|no}') = 'yes'
end;

function FromNormal: Boolean;
begin
	Result := FromUpdate = False
end;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: "..\Scraps.GUI\bin\Publish\win10-x64\Scraps.GUI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Scraps.GUI\bin\Publish\win10-x64\WebView2Loader.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Scraps\bin\Publish\win-x64\Scraps.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\license.txt"; DestDir: "{app}"; Flags: ignoreversion

[INI]
Filename: "{app}\Instructions.url"; Section: "InternetShortcut"; Key: "URL"; String: "http://bit.ly/scraps-instructions"
Filename: "{app}\Changelog.url"; Section: "InternetShortcut"; Key: "URL"; String: "http://bit.ly/scraps-changelog"
Filename: "{app}\Source Code.url"; Section: "InternetShortcut"; Key: "URL"; String: "http://bit.ly/scraps-repo"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\Instructions.url"; Description: "View instructions"; Flags: nowait postinstall skipifsilent shellexec unchecked
Filename: "{app}\Changelog.url"; Description: "View changelog"; Flags: nowait postinstall skipifsilent shellexec unchecked
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent; Check: FromUpdate
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent unchecked; Check: FromNormal

[UninstallDelete]
Type: files; Name: "{userdocs}\Caprine Logic\Scraps\Config.json"
Type: filesandordirs; Name: "{userdocs}\Caprine Logic\Scraps\Plugins\*"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps\Plugins"
Type: filesandordirs; Name: "{userdocs}\Caprine Logic\Scraps\Logs\*"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps\Logs"
Type: filesandordirs; Name: "{userdocs}\Caprine Logic\Scraps\Data\*"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps\Data"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps"
Type: files; Name: "{app}\Instructions.url"
Type: files; Name: "{app}\Changelog.url"
Type: files; Name: "{app}\Source Code.url"
Type: filesandordirs; Name: "{app}\Scraps.GUI.exe.WebView2\*"
Type: dirifempty; Name: "{app}\Scraps.GUI.exe.WebView2"
Type: dirifempty; Name: "{app}"