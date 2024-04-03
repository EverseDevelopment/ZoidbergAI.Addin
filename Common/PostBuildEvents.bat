rem Set the input arguments
set SolutionDir=%1
set SolutionName=%2
set ConfigurationName=%3
set TargetDir=%SolutionDir%%SolutionName%

rem Remove double quotes from the variables
set "SolutionDir=%SolutionDir:"=%"
set "SolutionName=%SolutionName:"=%"
set "ConfigurationName=%ConfigurationName:"=%"
set "TargetDir=%TargetDir:"=%"

set "BundleName=%SolutionName%.bundle"
set "BundlePath=%SolutionDir%\%BundleName%"
set "BundleContentsFolder=%BundlePath%\Contents"

set "ApplicationPluginsPath=%APPDATA%\Autodesk\ApplicationPlugins"
set "PluginDestinationPath=%ApplicationPluginsPath%\%BundleName%"

set "CertificatePath=%SolutionDir%\credentials.pfx"
set "SignToolPath="C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe""
set "InnoSetupPath=%SolutionDir%packages\Tools.InnoSetup.6.2.2\tools\ISCC.exe"
set "SignCommand=%SignToolPath% sign /f "%CertificatePath%" /p %CERTIFICATE_PASSWORD% /t http://timestamp.digicert.com /fd SHA256"

echo %SolutionDir%
echo %SolutionName%
echo %ConfigurationName%
echo %TargetDir%

echo %ApplicationPluginsPath%
echo %BundlePath%
echo %BundleContentsFolder%
echo %PluginDestinationPath%

echo %CertificatePath%
echo %SignToolPath%
echo %InnoSetupPath%
echo %SignCommand%

rem REMOVE EXISTING FILES
if EXIST "%BundlePath%" rmdir /s /q "%BundlePath%"

mkdir "%BundleContentsFolder%"
copy "%SolutionDir%\Common\PackageContents.xml" "%BundlePath%"

rem COPY MAIN FILES
for /L %%Y in (2019,1,2024) do (
    echo "-----------------------------%TargetDir%.%%Y-----------------------------"
    mkdir "%BundleContentsFolder%\%%Y\Images"
    mkdir "%BundleContentsFolder%\%%Y\Fonts"

    copy "%SolutionDir%\Revit.%%Y\bin\%ConfigurationName%\*.dll" "%BundleContentsFolder%\%%Y"
    copy "%SolutionDir%\Common\Images\*.png" "%BundleContentsFolder%\%%Y\Images"
    copy "%SolutionDir%\Common\Fonts\*.ttf" "%BundleContentsFolder%\%%Y\Fonts"

    copy "%SolutionDir%\Common\%SolutionName%.addin" "%BundleContentsFolder%\%%Y\"
)

rem Copy bundle
if %ConfigurationName% == Debug (
	xcopy /e /s /h /Y "%BundlePath%" "%PluginDestinationPath%"
)

if %ConfigurationName% == Release (
    
    if EXIST "%PluginDestinationPath%" rmdir /s /q "%PluginDestinationPath%"

    if EXIST "%CertificatePath%" (
        for /L %%Y in (2019,1,2024) do (
            echo "-----------------------------Signing %SolutionName%.dll %%Y-----------------------------"
            %SignCommand% "%BundleContentsFolder%\%%Y\%SolutionName%.dll"
        )
    )

    "%InnoSetupPath%" "%SolutionDir%Installer\InstallScript.iss"

    if EXIST "%CertificatePath%" (
        %SignCommand% "%SolutionDir%Installer\Installer\%SolutionName%.exe"
    )
)