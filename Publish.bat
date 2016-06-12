@echo off
@Set PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;%PATH:C:\Windows\Microsoft.NET\Framework\v4.0.30319;=%
echo restore packages...
nuget restore .\OpenData.sln
ECHO delete old files
rd /s /q .\Publish
MSBuild D:\Work\OpenData\Website\WebSite.WebApp\OpenData.WebSite.WebApp.csproj /p:DeployOnBuild=true /p:PublishProfile=Target.pubxml
