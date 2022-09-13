#define MyAppName "Scraps"
#define MyAppDescription "Scrap.TF Raffle Joiner"
#define MyAppVersion "5.1.0.0"
#define MyAppPublisher "Caprine Logic"
#define MyAppExeName "scraps.exe"
#define MyAppCopyright "Copyright (C) 2022 Caprine Logic"

[Setup]
AppId={{D3BD46E5-E1AF-41DA-92A3-4443B418294C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL=https://github.com/depthbomb
AppSupportURL=https://github.com/depthbomb/Scraps
AppUpdatesURL=https://github.com/depthbomb/Scraps/releases
AppCopyright={#MyAppCopyright}
VersionInfoVersion={#MyAppVersion}
DefaultDirName={autopf}\{#MyAppPublisher}\{#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
AllowNoIcons=yes
LicenseFile=.\license.txt
OutputDir=..\Scraps.Next\bin
OutputBaseFilename=scraps_setup
SetupIconFile=..\Scraps.Next\Resources\Scraps.ico
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
WizardResizable=no
WizardImageFile=.\images\Image_*.bmp
WizardSmallImageFile=.\images\SmallImage_*.bmp
ArchitecturesAllowed=x64
UninstallDisplayIcon={app}\scraps.exe
UninstallDisplayName={#MyAppName} - {#MyAppDescription}
ShowTasksTreeLines=True
AlwaysShowDirOnReadyPage=True
VersionInfoCompany={#MyAppPublisher}
VersionInfoCopyright={#MyAppCopyright}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}
VersionInfoProductTextVersion={#MyAppVersion}
VersionInfoDescription={#MyAppDescription}

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
Source: "..\Scraps.Next\bin\Publish\*"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\license.txt"; DestDir: "{app}"; Flags: ignoreversion

[INI]
Filename: "{app}\Changelog.url"; Section: "InternetShortcut"; Key: "URL"; String: "http://bit.ly/scraps-changelog"
Filename: "{app}\Source Code.url"; Section: "InternetShortcut"; Key: "URL"; String: "http://bit.ly/scraps-repo"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\Changelog.url"; Description: "View changelog"; Flags: nowait postinstall skipifsilent shellexec unchecked
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent; Check: FromUpdate
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent unchecked; Check: FromNormal

[UninstallDelete]
Type: files; Name: "{userdocs}\Caprine Logic\Scraps\Config.json"
Type: filesandordirs; Name: "{userdocs}\Caprine Logic\Scraps\Logs\*"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps\Logs"
Type: filesandordirs; Name: "{userdocs}\Caprine Logic\Scraps\Data\*"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps\Data"
Type: dirifempty; Name: "{userdocs}\Caprine Logic\Scraps"
Type: files; Name: "{app}\Changelog.url"
Type: files; Name: "{app}\Source Code.url"
Type: dirifempty; Name: "{app}"