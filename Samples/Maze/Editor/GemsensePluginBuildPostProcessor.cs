using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System;

class GemsensePluginBuildPostProcessor
{
    [PostProcessBuildAttribute()]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //Copy DLL from plugins folter to executable root for Windows builds
        if (target == BuildTarget.StandaloneWindows ||
           target == BuildTarget.StandaloneWindows64)
        {
            var pathToDLL = Path.Combine(Path.GetDirectoryName(pathToBuiltProject),
                            Path.GetFileNameWithoutExtension(pathToBuiltProject) + "_Data");

            pathToDLL = Path.Combine(pathToDLL, "Plugins");
            pathToDLL = Path.Combine(pathToDLL, "GemSDK.dll");

            var pathToCopy = Path.Combine(Path.GetDirectoryName(pathToBuiltProject), Path.GetFileName(pathToDLL));

            Debug.Log(string.Format("Copying \"{0}\" to \"{1}\"...", pathToDLL, pathToCopy));

            try
            {
                File.Copy(pathToDLL, pathToCopy, true);
                File.Delete(pathToDLL);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}
