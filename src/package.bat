set VERSION=1.1

build\NAnt\NAnt -f:build\nperf.build.xml -D:build.version=%VERSION% -D:build.configuration=Release package
pause