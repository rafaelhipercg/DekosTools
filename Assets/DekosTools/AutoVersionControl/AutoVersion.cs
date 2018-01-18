#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dekos.AutoVersionControler
{
    [InitializeOnLoad]
    public class AutoVersion : MonoBehaviour {
        static AutoVersion() {
            System.DateTime Date = System.DateTime.Now;
            string version = Date.Year.ToString().Remove(0, 2) + "." + Date.Month.ToString("00") + "." + Date.Day.ToString("00");
            if (!Directory.Exists("Assets/Resources"))
                Directory.CreateDirectory("Assets/Resources");
            StreamWriter SW = new StreamWriter("Assets/Resources/Ver.txt", false);
            SW.Write(version);
            SW.Close();
        }
    } }
#endif

