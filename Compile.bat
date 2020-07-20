::		Set Pathway for the current ".NET Framework"-directory
	echo 1.: Set default path for ".NET Framework"-directory
    set fdir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
	echo Done! && echo. && echo. && echo.

::Compile it with csc.exe
	echo 2.: Try to compile this with csc.exe
::		Set pathway for MSBuild.exe
		echo 2.1.: Set path to csc.exe
		set csc=%fdir%\csc.exe
		echo Done! && echo. && echo. && echo.

::		Compile by using csc.exe, to "\Compiled\rsaVault.exe".
		echo 2.2.: Compile with csc.exe, into \Compiled\rsaVault.exe
		%csc% -optimize -out:"Compiled\rsaVault.exe" "rsaVault\*.cs" /win32icon:"rsaVault\Encrypt.ico"
		echo Done! && echo. && echo. && echo.


::		Test console exe "Compiled\Console.bat", after compilation
		echo 2.3.: Test compiled rsaVault.exe
			echo 2.3.1.: Go to "Compiled"-folder
			cd Compiled
			echo Done! && echo. && echo. && echo.

			echo 2.3.2.: Run rsaVault as console-program
			start Console.bat
			echo Done! && echo. && echo. && echo.

::		Test by starting this as winexe, to show the window, after press any key.
			echo 2.3.3.: Run rsaVault as windows exe-program
			start rsaVault.exe
			echo Done! && echo. && echo. && echo.


::Compile it with MSBuild.exe
	echo 3.: Try to compile this with MSBuild.exe
::		Set pathway for MSBuild.exe

		echo 3.1.: Set path to MSBuild.exe
		set msbuild=%fdir%\MSBuild.exe
		echo Done! && echo. && echo. && echo.

::		Compile by using MSBuild, to "\rsaVault\bin\Release\rsaVault.exe" and "\rsaVault\obj\Release\rsaVault.exe".
		echo 3.2.: Compile with csc.exe, into "\rsaVault\bin\Releases\rsaVault.exe\" and "\rsaVault\obj\Releases\rsaVault.exe\"
		%msbuild% /property:Configuration=Release "..\rsaVault\rsaVault.csproj"
		echo Done! && echo. && echo. && echo.


::		Test start this from "bin\Release", and "obj\Release" folders:
		echo 3.3.: Test compiled rsaVault.exe

			echo 3.3.1.: Go to "..\rsaVault\"-folder
			cd ..\rsaVault
			echo Done! && echo. && echo. && echo.

			echo 3.3.2.: Test running this compiled program, from bin and obj folder
			test_from_bin_and_obj_release.bat
			echo Done! && echo. && echo. && echo.

pause