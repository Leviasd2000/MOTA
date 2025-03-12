set WORKSPACE=..\..
set LUBAN_DLL=%WORKSPACE%\LubanConfig\MiniTemplate\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
	-c cs-simple-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
	-x outputCodeDir=%WORKSPACE%\Assets\Code ^
	-x outputDataDir=%WORKSPACE%\Assets\Data

pause