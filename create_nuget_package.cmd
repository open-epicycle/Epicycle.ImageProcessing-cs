@echo off

rmdir NuGetPackage /s /q
mkdir NuGetPackage
mkdir NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0
mkdir NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib

copy package.nuspec NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\Epicycle.ImageProcessing-cs.0.1.1.0.nuspec
copy README.md NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\README.md
copy LICENSE NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\LICENSE

xcopy bin\net35\Release\Epicycle.ImageProcessing_cs.dll NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net35\
xcopy bin\net35\Release\Epicycle.ImageProcessing_cs.pdb NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net35\
xcopy bin\net35\Release\Epicycle.ImageProcessing_cs.xml NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net35\
xcopy bin\net40\Release\Epicycle.ImageProcessing_cs.dll NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net40\
xcopy bin\net40\Release\Epicycle.ImageProcessing_cs.pdb NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net40\
xcopy bin\net40\Release\Epicycle.ImageProcessing_cs.xml NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net40\
xcopy bin\net45\Release\Epicycle.ImageProcessing_cs.dll NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net45\
xcopy bin\net45\Release\Epicycle.ImageProcessing_cs.pdb NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net45\
xcopy bin\net45\Release\Epicycle.ImageProcessing_cs.xml NuGetPackage\Epicycle.ImageProcessing-cs.0.1.1.0\lib\net45\

cd NuGetPackage
nuget pack Epicycle.ImageProcessing-cs.0.1.1.0\Epicycle.ImageProcessing-cs.0.1.1.0.nuspec -Properties version=0.1.1.0
7z a -tzip Epicycle.ImageProcessing-cs.0.1.1.0.zip Epicycle.ImageProcessing-cs.0.1.1.0 Epicycle.ImageProcessing-cs.0.1.1.0.nupkg

echo nuget push Epicycle.ImageProcessing-cs.0.1.1.0.nupkg > push.cmd
echo pause >> push.cmd

pause