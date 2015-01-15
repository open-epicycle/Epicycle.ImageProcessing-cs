@echo off

cd projects
msbuild Epicycle.ImageProcessing.net35.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.ImageProcessing.net35.sln /t:Clean,Build /p:Configuration=Release
msbuild Epicycle.ImageProcessing.net40.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.ImageProcessing.net40.sln /t:Clean,Build /p:Configuration=Release
msbuild Epicycle.ImageProcessing.net45.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.ImageProcessing.net45.sln /t:Clean,Build /p:Configuration=Release

pause
