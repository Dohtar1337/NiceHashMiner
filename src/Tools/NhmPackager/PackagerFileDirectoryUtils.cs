﻿using NHM.Common;
using System.Diagnostics;
using System.IO;

namespace NhmPackager
{
    internal static class PackagerFileDirectoryUtils
    {
        public static void DeletePathIfExists(string path)
        {
            if (File.Exists(path)) File.Delete(path);
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }

        public static void RecreateDirectoryIfExists(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Logger.Info("RecreateDirectoryIfExists", $"Deleting '{dirPath}'");
                Directory.Delete(dirPath, true);
            }
            Paths.EnsureDirectoryPath(dirPath);
        }

        public static bool ExecXCopy(string copyFrom, string copyTo)
        {
            using var copyRelease = new Process {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "xcopy.exe",
                    Arguments = $"/s /i {copyFrom} {copyTo}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            var ok = copyRelease.Start();
            while (!copyRelease.StandardOutput.EndOfStream)
            {
                string line = copyRelease.StandardOutput.ReadLine();
                Logger.Info("ExecXCopy", line);
            }
            return ok && copyRelease.ExitCode == 0;
        }
    }
}
