using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace DekosTools.Language {
    public class LanguageEditor : EditorWindow
    {
        public SystemLanguage Default;
        LanguageDictionary languagedictionary;

        [MenuItem("DekosTools/Language Editor")]
        static void Init()
        {
            GetWindow(typeof(LanguageEditor)).Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Language Settings", EditorStyles.boldLabel);
            Default = (SystemLanguage)EditorGUILayout.EnumPopup("Default language", Default);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save")) save();
            if (GUILayout.Button("Load")) load();
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("Language Texts", EditorStyles.boldLabel);

        }

        void save() {
            Debug.Log("Save");
        }

        void load()
        {
            Debug.Log("Save");
        }

    } }