# Netstrum

This project uses [vcpkg](https://github.com/microsoft/vcpkg) for dependency management.

## Installing vcpkg

```bash
# Clone vcpkg
git clone https://github.com/microsoft/vcpkg.git
cd vcpkg

# Bootstrap vcpkg
./bootstrap-vcpkg.sh  # or .\bootstrap-vcpkg.bat on Windows

# Install dependencies for this project
./vcpkg install --triplet x64-windows # adjust triplet as needed
```

## CMake configuration

When configuring the project with CMake, set the `CMAKE_TOOLCHAIN_FILE` to use vcpkg's build system file:

```bash
cmake -S . -B build -DCMAKE_TOOLCHAIN_FILE=/path/to/vcpkg/scripts/buildsystems/vcpkg.cmake
cmake --build build
```

Replace `/path/to/vcpkg` with the path where vcpkg was cloned.
