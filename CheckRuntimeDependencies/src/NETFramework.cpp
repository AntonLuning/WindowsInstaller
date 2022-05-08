#include "NETFramework.h"

#include <windows.h>
#include <winreg.h>
#include <shellapi.h>
#include <stdexcept>


namespace RuntimeCheck {

    bool NETFramework::CheckIfVersionIsInstalled()
    {
        if (_version == 0)
            throw std::invalid_argument(".NET version is not set.");

        bool isInstalled = true;

        HKEY hKey;
        LONG retValue = RegOpenKeyExW(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full", 0, KEY_READ, &hKey);

        if (retValue != ERROR_SUCCESS)
        {
            if (retValue == ERROR_FILE_NOT_FOUND)
                isInstalled = false;    // .NET Framework 4.5 or later is not installed
            else 
            {
                MessageBox(
                    NULL,
                    (LPCWSTR)L"Unknown error. Failed to run the installation wizard.",
                    (LPCWSTR)L"Install Error",
                    MB_ICONWARNING | MB_OK
                );

                return false;
            }
        }
        else
        {
            DWORD version;
            unsigned long type = REG_DWORD, size = 1024;
            retValue = RegQueryValueExW(hKey, L"Release", NULL, &type, (LPBYTE)&version, &size);

            if (retValue != ERROR_SUCCESS)
            {
                MessageBox(
                    NULL,
                    (LPCWSTR)L"Unknown error. Failed to run the installation wizard.",
                    (LPCWSTR)L"Install Error",
                    MB_ICONWARNING | MB_OK
                );

                return false;
            }
            else if (version < _version)
                isInstalled = false;    // Installed .NET Framework version is not "high enough"

            RegCloseKey(hKey);
        }

        if (!isInstalled)
            DisplayNotInstalledMessageBox();

        return isInstalled;
    }

    void NETFramework::DisplayNotInstalledMessageBox()
    {
        std::string sTemp = ".NET Framework " + GetVersionString() + " (or later) is required.\n\nInstall it and run the setup again!";
        std::wstring information = std::wstring(sTemp.begin(), sTemp.end());

        int msgboxID = MessageBox(
            NULL,
            information.c_str(),
            (LPCWSTR)L"Install Error",
            MB_ICONWARNING | MB_OKCANCEL | MB_DEFBUTTON1
        );

        if (msgboxID == IDOK)
            ShellExecute(0, 0, L"https://dotnet.microsoft.com/en-us/download/dotnet-framework", 0, 0, SW_SHOW);
    }

    std::string NETFramework::GetVersionString()
    {
        if (_version == 0)
            throw std::invalid_argument(".NET version is not set.");

        switch (_version)
        {
        case 528372:
            return "4.8";
        case 461808:
            return "4.7.2";
        case 461308:
            return "4.7.1";
        case 460798:
            return "4.7";
        case 394802:
            return "4.6.2";
        case 394254:
            return "4.6.1";
        case 393295:
            return "4.6";
        case 379893:
            return "4.5.2";
        case 378675:
            return "4.5.1";
        case 378389:
            return "4.5";
        }

        return "";
    }

}