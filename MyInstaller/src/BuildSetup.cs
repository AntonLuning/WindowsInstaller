using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInstaller
{
    public static class BuildSetup
    {
        public static bool GenerateFile()
        {
            string slnDirectory = GetSolutionDirectoryPath();
            if (Path.GetFileName(slnDirectory) == string.Empty)
                return false;

            List<string> swFilePaths = GetSetupWiardFilesPath(slnDirectory);
            if (swFilePaths.Count == 0)
                return false;

            string crdFilePath = GetCheckRuntimeDependencyFilePath(slnDirectory);
            if (crdFilePath == string.Empty)
                return false;

            // TODO: Get application files

            // TODO: Get application data files (and if they are local or roaming)

            // TODO: Build SFX

            return true;
        }

        private static string GetSolutionDirectoryPath()
        {
            string thisExePath = Path.TrimEndingDirectorySeparator(AppDomain.CurrentDomain.BaseDirectory);

            string dir = Directory.GetParent(thisExePath).FullName;
            while (Path.GetFileName(dir) != "WindowsInstaller" && Path.GetFileName(dir) != string.Empty)
                dir = Directory.GetParent(dir).FullName;

            return dir;
        }

        private static List<string> GetSetupWiardFilesPath(string slnDir)
        {
#if DEBUG
            string dir = Path.Combine(slnDir, @"SetupWizard.GUI\bin\Debug\net6.0-windows");
#else
            string dir = Path.Combine(slnDir, @"SetupWizard.GUI\bin\Release\net6.0-windows");
#endif

            List<string> filePaths = new List<string>();
            if (Directory.Exists(dir))
                foreach (string file in Directory.GetFiles(dir))
                {
                    string fileExt = Path.GetExtension(file);
                    if (fileExt.ToLower() == ".exe" || fileExt.ToLower() == ".dll")
                        filePaths.Add(file);
                }

            return filePaths;
        }

        private static string GetCheckRuntimeDependencyFilePath(string slnDir)
        {
#if DEBUG
            string dir = Path.Combine(slnDir, @"CheckRuntimeDependencies\bin\Debug");
#else
            string dir = Path.Combine(slnDir, @"CheckRuntimeDependencies\bin\Release");
#endif

            string filePath = Path.Combine(dir, "CheckRuntimeDependencies.exe");
            if (!File.Exists(filePath))
                return string.Empty;

            return filePath;
        }
    }
}