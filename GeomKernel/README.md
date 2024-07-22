# VENTUS_GeomKernel

VENTUS_GeomKernel provides a geometry library based on OCC.

    1. Before building the project solution, you need to install the possible version of OCC library, which is OpenCASCADE-7.7.0.
    The required OCC library can be dowloaded here: [text](https://dev.opencascade.org/release/previous) 
    2. Setup the OCC-Path on your OS, and name it 'OCC_PATH'
    3. Then in each TEST project you have to set up the Evironment path: Properties->Debugging->Environment.
    There, if you have istalled OCC on WINDOWS in C-folder, it should be similar to 
        PATH=C:\OpenCASCADE-7.7.0-vc14-64\ffmpeg-3.3.4-64\bin;C:\OpenCASCADE-7.7.0-vc14-64\freetype-2.5.5-vc14-64\bin;C:\OpenCASCADE-7.7.0-vc14-64\freeimage-3.17.0-vc14-64\bin;C:\OpenCASCADE-7.7.0-vc14-64\openvr-1.14.15-64\bin\win64;C:\OpenCASCADE-7.7.0-vc14-64\opencascade-7.7.0\win64\vc14\bin;C:\OpenCASCADE-7.7.0-vc14-64\tbb_2021.5-vc14-64\bin;..\VENTUS_GeomKernel\dll;%PATH%
    4. After above steps you should be able to build the whole solution and run the tests.