# Netstrum Hook DLL

This Windows DLL uses [Microsoft Detours](https://github.com/microsoft/Detours) to intercept `send` and `recv` calls and forward packet data to the Netstrum GUI via a TCP connection.

## Build

```powershell
cmake -S .. -B build -DCMAKE_TOOLCHAIN_FILE=%VCPKG_ROOT%/scripts/buildsystems/vcpkg.cmake
cmake --build build --target netstrum_hook
```

## Injection

Load the compiled `netstrum_hook.dll` into the target process using a helper injector or tools like `rundll32`.
