#include "NETFramework.h"

#include <windows.h>


void StartInstallerSetup()
{
    LPCTSTR setupPath = L"temp\setup.exe";  // TODO: Retrieve correct temp path for install files (where they were unpacked)

    STARTUPINFO startInfo;
    PROCESS_INFORMATION processInfo;

    ZeroMemory(&startInfo, sizeof(startInfo));
    startInfo.cb = sizeof(startInfo);
    ZeroMemory(&processInfo, sizeof(processInfo));

    bool setupProcess = CreateProcess(
        setupPath,      // Setup.exe full path
        NULL,           // Command line arguments
        NULL,           // Process handle not inheritable
        NULL,           // Thread handle not inheritable
        FALSE,          // Set handle inheritance to FALSE
        0,              // No creation flags
        NULL,           // Use parent's environment block
        NULL,           // Use parent's starting directory 
        &startInfo,     // Pointer to STARTUPINFO structure
        &processInfo    // Pointer to PROCESS_INFORMATION structure
    );

    if (!setupProcess)
    {
        // TODO: delete temp folder with install files

        MessageBox(
            NULL,
            (LPCWSTR)L"Unknown error. Failed to run the installation wizard.",
            (LPCWSTR)L"Install Error",
            MB_ICONWARNING | MB_OK
        );
    }

    CloseHandle(processInfo.hProcess);   // Close process handle
    CloseHandle(processInfo.hThread);    // Close thread handle
}

int main()
{
    // .NET Framework check
    if (true)   // TODO: Check generated constructor file if there is any .NET Framework dependency required
    {
        auto NET = RuntimeCheck::NETFramework();
        NET.SetTargetedVersion(NET.NET_4_7_2);      // TODO: Set the lowest desired version from generated constructor file
        if (!NET.CheckIfVersionIsInstalled())
            return 1;
    }

    StartInstallerSetup();

    return 0;
}