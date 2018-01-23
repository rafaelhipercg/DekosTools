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

//AutoBuildVersionCounter V1.0.0

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using System.IO;
using DekosTools.VersionControl;
class AutoBuildVersionCounter : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildTarget target, string path)
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
        int.TryParse(VersionData.Version.Replace(".", "") + VersionData.Build, out Ver);
        PlayerSettings.Android.bundleVersionCode = Ver;
        PlayerSettings.iOS.buildNumber = Ver.ToString();
        PlayerSettings.bundleVersion = VersionData.Version + "." + VersionData.Build;
    }
}
 #endif