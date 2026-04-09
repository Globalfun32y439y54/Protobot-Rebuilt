[Setup]
AppName=Protobot Rebuilt
AppVersion=1.3.6
DefaultDirName={autopf}\ProtobotRebuilt
DefaultGroupName=Protobot Rebuilt
OutputBaseFilename=Protobot-Rebuilt-Setup
OutputDir=..\Output
Compression=lzma
SolidCompression=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Files]
; Grabs all files from your Unity Build folder
Source: "..\Builds\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Creates the Start Menu shortcut
Name: "{group}\Protobot Rebuilt"; Filename: "{app}\Protobot Rebuilt.exe"
; Creates the Desktop shortcut
Name: "{commondesktop}\Protobot Rebuilt"; Filename: "{app}\Protobot Rebuilt.exe"