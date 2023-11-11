#!/bin/bash

destDir=../../build/Gemstone.POSIX

# Define absolute path for script log
log=${destDir}/Gemstone.POSIX-build-log.txt

if [ "$(id -u)" != "0" ]; then
    echo "ERROR: Operation must execute as root." 1>&2
    exit 1
fi

echo
echo "Make sure gcc compiler and pam libraries are installed, e.g.:"
echo "    > sudo apt install build-essential"
echo "    > sudo apt install libpam0g-dev"
echo
read -p "Are you ready to continue (y/N)? " -r -n 1
echo

if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    exit 1
fi

if [ ! -d "$destDir" ]; then
    mkdir -p "$destDir"
fi

# Create log file
echo "Gemstone.POSIX Build Log" > ${log}
date >> ${log}

if [ ${OSTYPE} != ${OSTYPE#darwin} ]; then
    echo "Mac operating system detected..." >> ${log}
    isMac=1
else
    echo "Linux operating system assumed..." >> ${log}
    isMac=0
fi

# Build Gemstone POSIX library
if [ $isMac -ne 0 ]; then
    sudo gcc -m32 -c -Wall -Werror -fpic Gemstone.POSIX.c >> ${log}
    if [ $? -ne 0 ]
    then
        echo "Failed to compile Gemstone POSIX library"
        exit 1
    fi

    sudo gcc -m32 -dynamiclib -undefined suppress -flat_namespace Gemstone.POSIX.o -o Gemstone.POSIX.so -lpam >> ${log}
    if [ $? -ne 0 ]
    then
        echo "Failed to build Gemstone POSIX dynamic library"
        exit 1
    fi
else
    gcc -c -Wall -Werror -fpic Gemstone.POSIX.c >> ${log}
    if [ $? -ne 0 ]
    then
        echo "Failed to compile Gemstone POSIX library"
        exit 1
    fi

    gcc -shared -o Gemstone.POSIX.so Gemstone.POSIX.o -lpam -lpam_misc -lcrypt >> ${log}
    if [ $? -ne 0 ]
    then
        echo "Failed to build Gemstone POSIX shared library"
        exit 1
    fi
fi

echo "Successfully built Gemstone POSIX shared library"
echo "Successfully built Gemstone POSIX shared library" >> ${log}

cp -v Gemstone.POSIX.so ${destDir} >> ${log}
