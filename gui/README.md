# Netstrum GUI

This directory contains a simple WPF application used to interact with Netstrum.

## Building

1. Ensure the .NET SDK 6.0 or later is installed.
2. Restore NuGet packages:
   ```sh
   dotnet restore
   ```
3. Build the project:
   ```sh
   dotnet build
   ```

When building on non-Windows platforms, append `-p:EnableWindowsTargeting=true` to the commands.
