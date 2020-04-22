using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;


namespace Cheky.Editor
{
    // This script just updates any build .exe modified date to the current date at time of build
    public class CopyDataFilesPostprocessor
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            File.SetLastWriteTime(pathToBuiltProject, DateTime.Now);
        }
    }
}