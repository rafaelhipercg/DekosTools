/*
MIT License

Copyright (c) 2017 Rafael cosentino garcia

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//AutoBuildVersionCounter V0.0.2

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace DekosTools.VersionControl
{
    public class AutoBuildVersionCounter
    {
        public static void BuildGame(BuildOptions Options = BuildOptions.None)
        {
            if (!Directory.Exists("Assets/Resources"))
                Directory.CreateDirectory("Assets/Resources");

            int Count = 1;
            if (File.Exists("Assets/Resources/Build.txt"))
            {
                StreamReader SR = new StreamReader("Assets/Resources/Build.txt", false);
                int.TryParse(SR.ReadLine(), out Count);
                SR.Close();
                Count++;
            }
            StreamWriter SW = new StreamWriter("Assets/Resources/Build.txt", false);
            SW.Write(string.Format("{0:0000}", Count));
            SW.Close();

            AssetDatabase.Refresh();

            int Ver;
            int.TryParse(VersionData.Version.Replace(".", "") + VersionData.Build,out Ver);
            PlayerSettings.Android.bundleVersionCode = Ver;
            PlayerSettings.iOS.buildNumber =Ver.ToString();
            PlayerSettings.bundleVersion = VersionData.Version + "." + VersionData.Build;

            string fileNameExtension = "";

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    fileNameExtension = ".apk";
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    fileNameExtension = ".exe";
                    break;
                case BuildTarget.StandaloneOSX:
                    fileNameExtension = ".app";
                    break;
                default:
                    fileNameExtension = "";
                    break;
            }

            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "/Build", "");
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + "/" + PlayerSettings.productName + fileNameExtension, EditorUserBuildSettings.activeBuildTarget, Options);
        }

        [MenuItem("DekosTools/AutoVersion/Build")]
        public static void BuildGame_Build()
        {
            BuildGame();
        }

        [MenuItem("DekosTools/AutoVersion/Build And Run")]
        public static void BuildGame_AutoRunPlayer()
        {
            BuildGame(BuildOptions.AutoRunPlayer);
        }

        [MenuItem("DekosTools/AutoVersion/Options/AcceptExternalModificationsToPlayer")]
        public static void BuildGame_AcceptExternalModificationsToPlayer()
        {
            BuildGame(BuildOptions.AcceptExternalModificationsToPlayer);
        }

        [MenuItem("DekosTools/AutoVersion/Options/AllowDebugging")]
        public static void BuildGame_AllowDebugging()
        {
            BuildGame(BuildOptions.AllowDebugging);
        }

        [MenuItem("DekosTools/AutoVersion/Options/BuildAdditionalStreamedScenes")]
        public static void BuildGame_BuildAdditionalStreamedScenes()
        {
            BuildGame(BuildOptions.BuildAdditionalStreamedScenes);
        }

        [MenuItem("DekosTools/AutoVersion/Options/BuildScriptsOnly")]
        public static void BuildGame_BuildScriptsOnly()
        {
            BuildGame(BuildOptions.BuildScriptsOnly);
        }

        [MenuItem("DekosTools/AutoVersion/Options/ComputeCRC")]
        public static void BuildGame_CompressTextures()
        {
            BuildGame(BuildOptions.ComputeCRC);
        }

        [MenuItem("DekosTools/AutoVersion/Options/ConnectToHost")]
        public static void BuildGame_ConnectToHost()
        {
            BuildGame(BuildOptions.ConnectToHost);
        }

        [MenuItem("DekosTools/AutoVersion/Options/ConnectWithProfiler")]
        public static void BuildGame_ConnectWithProfiler()
        {
            BuildGame(BuildOptions.ConnectWithProfiler);
        }

        [MenuItem("DekosTools/AutoVersion/Options/Development")]
        public static void BuildGame_Development()
        {
            BuildGame(BuildOptions.Development);
        }

        [MenuItem("DekosTools/AutoVersion/Options/EnableHeadlessMode")]
        public static void BuildGame_EnableHeadlessMode()
        {
            BuildGame(BuildOptions.EnableHeadlessMode);
        }

        [MenuItem("DekosTools/AutoVersion/Options/ForceEnableAssertions")]
        public static void BuildGame_ForceEnableAssertions()
        {
            BuildGame(BuildOptions.ForceEnableAssertions);
        }

        [MenuItem("DekosTools/AutoVersion/Options/ForceOptimizeScriptCompilation")]
        public static void BuildGame_ForceOptimizeScriptCompilation()
        {
            BuildGame(BuildOptions.ForceOptimizeScriptCompilation);
        }

        [MenuItem("DekosTools/AutoVersion/Options/Il2CPP")]
        public static void BuildGame_Il2CPP()
        {
            BuildGame(BuildOptions.Il2CPP);
        }

        [MenuItem("DekosTools/AutoVersion/Options/InstallInBuildFolder")]
        public static void BuildGame_InstallInBuildFolder()
        {
            BuildGame(BuildOptions.InstallInBuildFolder);
        }

        [MenuItem("DekosTools/AutoVersion/Options/ShowBuiltPlayer")]
        public static void BuildGame_ShowBuiltPlayer()
        {
            BuildGame(BuildOptions.ShowBuiltPlayer);
        }

        [MenuItem("DekosTools/AutoVersion/Options/StrictMode")]
        public static void BuildGame_StrictMode()
        {
            BuildGame(BuildOptions.StrictMode);
        }

        [MenuItem("DekosTools/AutoVersion/Options/SymlinkLibraries")]
        public static void BuildGame_SymlinkLibraries()
        {
            BuildGame(BuildOptions.SymlinkLibraries);
        }

        [MenuItem("DekosTools/AutoVersion/Options/UncompressedAssetBundle")]
        public static void BuildGame_UncompressedAssetBundle()
        {
            BuildGame(BuildOptions.UncompressedAssetBundle);
        }
    }
}

#endif
