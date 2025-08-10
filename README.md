# Netstrum

Netstrum is a modular framework for experimenting with network packet instrumentation.
The project is split into multiple subprojects that work together to provide a
flexible platform for logging, observing and manipulating network traffic.

## Architecture

The repository is organized into several components:

- **Core library** – shared logic that powers packet observation and command
  processing.  It is built as the `netstrum_dll` library.
- **dll/** – planned native extension containing packet instrumentation hooks.
- **gui/** – planned graphical interface for interacting with the instrumentation
  layer.

## Build requirements

Netstrum uses a modern C++ toolchain and relies on the following tools:

- [CMake](https://cmake.org/) for generating project files.
- [vcpkg](https://github.com/microsoft/vcpkg) for dependency management.
- [Visual Studio](https://visualstudio.microsoft.com/) on Windows for building and
  debugging.

## Usage overview

1. Install the required tools and ensure `vcpkg` is integrated with CMake.
2. Generate build files, for example:
   ```bash
   cmake -S . -B build -DCMAKE_TOOLCHAIN_FILE="<path to vcpkg.cmake>"
   ```
3. Build the desired targets using your chosen build system (e.g. `cmake --build build`).

The `dll` and `gui` directories currently contain placeholders and will gain
CMake build scripts and functionality in future revisions.
