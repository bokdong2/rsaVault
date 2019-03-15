::    C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe -target:winexe -optimize -out:"Compiled\rsaVault.exe" "rsaVault\*.cs"

::after compilation without -target:winexe is available the console window
    C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe -optimize -out:"Compiled\rsaVault.exe" "rsaVault\*.cs" /win32icon:"rsaVault\Encrypt.ico"

::Start "Compiled\Console.bat", after compilation
	cd Compiled
	Console.bat

::    C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /property:Configuration=Release "rsaVault\rsaVault.csproj"

