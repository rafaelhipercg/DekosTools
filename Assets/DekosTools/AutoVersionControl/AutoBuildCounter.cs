#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace Dekos.AutoVersionControler
{
    //Vou mudar na 3.6
    public class AutoBuildCounter
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
            SW.Write(Count);
            SW.Close();

            AssetDatabase.Refresh();

            string fileNameExtension = "";

            switch (EditorUserBuildSettings.selectedStandaloneTarget)
            {
                case BuildTarget.Android:
                    fileNameExtension = ".apk";
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    fileNameExtension = ".exe";
                    break;
                case BuildTarget.StandaloneOSXUniversal:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXIntel:
                    fileNameExtension = ".app";
                    break;
                default:
                    fileNameExtension = "";
                    break;
            }

            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "/Build", "");
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + "/" + PlayerSettings.productName + fileNameExtension, EditorUserBuildSettings.selectedStandaloneTarget, Options);
        }

        [MenuItem("Dekos/AutoVersion/Build")]
        public static void BuildGame_Build()
        {
            BuildGame();
        }

        [MenuItem("Dekos/AutoVersion/Build And Run")]
        public static void BuildGame_AutoRunPlayer()
        {
            BuildGame(BuildOptions.AutoRunPlayer);
        }

        [MenuItem("Dekos/AutoVersion/Options/AcceptExternalModificationsToPlayer")]
        public static void BuildGame_AcceptExternalModificationsToPlayer()
        {
            BuildGame(BuildOptions.AcceptExternalModificationsToPlayer);
        }

        [MenuItem("Dekos/AutoVersion/Options/AllowDebugging")]
        public static void BuildGame_AllowDebugging()
        {
            BuildGame(BuildOptions.AllowDebugging);
        }

        [MenuItem("Dekos/AutoVersion/Options/BuildAdditionalStreamedScenes")]
        public static void BuildGame_BuildAdditionalStreamedScenes()
        {
            BuildGame(BuildOptions.BuildAdditionalStreamedScenes);
        }

        [MenuItem("Dekos/AutoVersion/Options/BuildScriptsOnly")]
        public static void BuildGame_BuildScriptsOnly()
        {
            BuildGame(BuildOptions.BuildScriptsOnly);
        }

        [MenuItem("Dekos/AutoVersion/Options/ComputeCRC")]
        public static void BuildGame_CompressTextures()
        {
            BuildGame(BuildOptions.ComputeCRC);
        }

        [MenuItem("Dekos/AutoVersion/Options/ConnectToHost")]
        public static void BuildGame_ConnectToHost()
        {
            BuildGame(BuildOptions.ConnectToHost);
        }

        [MenuItem("Dekos/AutoVersion/Options/ConnectWithProfiler")]
        public static void BuildGame_ConnectWithProfiler()
        {
            BuildGame(BuildOptions.ConnectWithProfiler);
        }

        [MenuItem("Dekos/AutoVersion/Options/Development")]
        public static void BuildGame_Development()
        {
            BuildGame(BuildOptions.Development);
        }

        [MenuItem("Dekos/AutoVersion/Options/EnableHeadlessMode")]
        public static void BuildGame_EnableHeadlessMode()
        {
            BuildGame(BuildOptions.EnableHeadlessMode);
        }

        [MenuItem("Dekos/AutoVersion/Options/ForceEnableAssertions")]
        public static void BuildGame_ForceEnableAssertions()
        {
            BuildGame(BuildOptions.ForceEnableAssertions);
        }

        [MenuItem("Dekos/AutoVersion/Options/ForceOptimizeScriptCompilation")]
        public static void BuildGame_ForceOptimizeScriptCompilation()
        {
            BuildGame(BuildOptions.ForceOptimizeScriptCompilation);
        }

        [MenuItem("Dekos/AutoVersion/Options/Il2CPP")]
        public static void BuildGame_Il2CPP()
        {
            BuildGame(BuildOptions.Il2CPP);
        }

        [MenuItem("Dekos/AutoVersion/Options/InstallInBuildFolder")]
        public static void BuildGame_InstallInBuildFolder()
        {
            BuildGame(BuildOptions.InstallInBuildFolder);
        }

        [MenuItem("Dekos/AutoVersion/Options/ShowBuiltPlayer")]
        public static void BuildGame_ShowBuiltPlayer()
        {
            BuildGame(BuildOptions.ShowBuiltPlayer);
        }

        [MenuItem("Dekos/AutoVersion/Options/StrictMode")]
        public static void BuildGame_StrictMode()
        {
            BuildGame(BuildOptions.StrictMode);
        }

        [MenuItem("Dekos/AutoVersion/Options/SymlinkLibraries")]
        public static void BuildGame_SymlinkLibraries()
        {
            BuildGame(BuildOptions.SymlinkLibraries);
        }

        [MenuItem("Dekos/AutoVersion/Options/UncompressedAssetBundle")]
        public static void BuildGame_UncompressedAssetBundle()
        {
            BuildGame(BuildOptions.UncompressedAssetBundle);
        }
    }
}

#endif
