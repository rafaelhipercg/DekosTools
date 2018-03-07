#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Swing.Editor;
using SimpleJSON;

namespace DekosTools.LanguageTranslate
{
    public class LanguageEditor : EditorWindow
    {
        [SerializeField]
        static public SystemLanguage Default = SystemLanguage.English;
        [SerializeField]
        static SystemLanguage Selected = SystemLanguage.English;
        [SerializeField]
        static LanguageDictionary languagedictionary;
        static private string Filename = "Language";

        public enum TypeSelectipon
        {
            Include,
            Exclude
        }
        TypeSelectipon typeSelection = TypeSelectipon.Include;
        static SystemLanguage FromLanguage
        {
            get
            {
                return Default;
            }
            set
            {
                Default = value;
            }
        }
        static SystemLanguage ToLanguage
        {
            get
            {
                return Selected;
            }
            set
            {
                Selected = value;
            }
        }

        bool[] OpenField = new bool[100000];
        Vector2 scrollPos;
        Vector2 scrollPos2;
        bool[] OpenTools = new bool[10];

        bool[] ToNotTranslete = new bool[43];
        [MenuItem("DekosTools/Language Editor")]
        static void Init()
        {
            GetWindow(typeof(LanguageEditor)).titleContent = new GUIContent("Language");
            Selected = Default;
            if (languagedictionary == null)
            {
                languagedictionary = new LanguageDictionary();
            }
            GetWindow(typeof(LanguageEditor)).Show();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void OnGUI()
        {

            EditorGUI.BeginChangeCheck();
            {

                if (languagedictionary == null)
                {
                    load();
                }
                else
                if (languagedictionary.languageContent[(int)Selected] == null)
                {
                    languagedictionary.languageContent[(int)Selected] = new LanguageDictionaryContent();
                }
                GUILayout.Label("Language Settings", EditorStyles.boldLabel);
                Default = (SystemLanguage)EditorGUILayout.EnumPopup("Default language", Default);
                Filename = EditorGUILayout.TextField("File name:", Filename);
                if (Filename != "Language")
                {
                    EditorGUILayout.HelpBox("Language is the default file name, if changed will not be loaded by LanguageSever. it is recommended to rename only for archiving", MessageType.Warning, true);
                }
                EditorGUILayout.Space();
                GUILayout.Label("Language Dictionary", EditorStyles.boldLabel);
                Selected = (SystemLanguage)EditorGUILayout.EnumPopup("Language", Selected);
                languagedictionary.languageContent[(int)Selected].Enable = EditorGUILayout.Toggle("Enable", languagedictionary.languageContent[(int)Selected].Enable);
                if (!languagedictionary.languageContent[(int)Selected].Enable)
                {
                    EditorGUILayout.HelpBox("When disable it will return to the default language, which is " + Default.ToString(), MessageType.Info, true);
                }
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1);
                scrollPos =
                EditorGUILayout.BeginScrollView(scrollPos, "Box");
                {
                    GUI.backgroundColor = Color.white;
                    if (languagedictionary.languageContent[(int)Selected] != null)
                    {
                        for (int i = 0; i < languagedictionary.languageContent[(int)Selected].Entitys.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                OpenField[i] = EditorGUILayout.Foldout(OpenField[i], languagedictionary.languageContent[(int)Selected].Entitys[i].Key != "" ? languagedictionary.languageContent[(int)Selected].Entitys[i].Key : "No key");
                                GUI.backgroundColor = new Color(0.5f, 1, 1, 1);
                                if (GUILayout.Button("t", GUILayout.Width(16), GUILayout.Height(16))) TranslateText(i);
                                GUI.backgroundColor = new Color(1, 0.5f, 0.5f, 1);
                                if (GUILayout.Button("X", GUILayout.Width(16), GUILayout.Height(16))) del(i);
                                GUI.backgroundColor = Color.white;
                            }
                            EditorGUILayout.EndHorizontal();
                            if (OpenField[i])
                            {
                                EditorGUILayout.BeginVertical("Box");
                                {
                                    EditorGUILayout.LabelField("Key:");
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        languagedictionary.languageContent[(int)Selected].Entitys[i].Key =
                                        EditorGUILayout.TextField(languagedictionary.languageContent[(int)Selected].Entitys[i].Key);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.Space();
                                    EditorGUILayout.LabelField("Text:");
                                    languagedictionary.languageContent[(int)Selected].Entitys[i].Text = EditorGUILayout.TextArea(languagedictionary.languageContent[(int)Selected].Entitys[i].Text);
                                    EditorGUILayout.Space();
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                        if (languagedictionary.languageContent[(int)Selected].Entitys.Count == 0)
                        {
                            EditorGUILayout.HelpBox("No entries, click \"add\" to add an entry", MessageType.Info, true);
                        }
                    }
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(0.5f, 1, 0.5f, 1);
                    if (GUILayout.Button("Add")) add();
                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.Space();
                GUILayout.Label("Tools", EditorStyles.boldLabel);

                OpenTools[0] = EditorGUILayout.Foldout(OpenTools[0], "Auto translate");
                if (OpenTools[0])
                {
                    if (!Translating)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        {
                            FromLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("From  (Default)", FromLanguage);
                            ToLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("To  (Selected)", ToLanguage);
                            EditorGUILayout.BeginHorizontal();
                            {
                                GUI.backgroundColor = new Color(0.5f, 1, 1, 1);
                                if (GUILayout.Button("Auto translate")) TranslateText();
                                GUI.backgroundColor = Color.white;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.BeginVertical("Box");
                        {
                            GUILayout.Label(TranslateTitle);
                            GUILayout.Label(TranslateInfo);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                OpenTools[1] = EditorGUILayout.Foldout(OpenTools[1], "Copy");
                if (OpenTools[1])
                {
                    EditorGUILayout.BeginVertical("Box");
                    {
                        FromLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("From  (Default)", FromLanguage);
                        ToLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("To  (Selected)", ToLanguage);
                        if (GUILayout.Button("Copy")) CopyText();
                    }
                    EditorGUILayout.EndVertical();
                }

                OpenTools[2] = EditorGUILayout.Foldout(OpenTools[2], "Copy and Auto translate");
                if (OpenTools[2])
                {
                    EditorGUILayout.BeginVertical("Box");
                    FromLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("From  (Default)", FromLanguage);
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("To");
                            typeSelection = (TypeSelectipon)EditorGUILayout.EnumPopup(typeSelection);
                        }
                        EditorGUILayout.EndHorizontal();
                        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, "Box");
                    }
                    for (int i = 0; i < 43; i++)
                    {
                        if ((SystemLanguage)i != Default && i != (int)SystemLanguage.Unknown)
                        {
                            if (typeSelection == TypeSelectipon.Exclude)
                            {
                                ToNotTranslete[i] = EditorGUILayout.Toggle(((SystemLanguage)i).ToString(), ToNotTranslete[i]);
                            }
                            else
                            {
                                ToNotTranslete[i] = !EditorGUILayout.Toggle(((SystemLanguage)i).ToString(), !ToNotTranslete[i]);
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                    {
                        if (GUILayout.Button("Copy and Auto translate")) CopyAndTranslateAllText();
                    }
                    EditorGUILayout.EndVertical();
                }

                OpenTools[3] = EditorGUILayout.Foldout(OpenTools[3], "Key to Text");
                if (OpenTools[3])
                {
                    if (GUILayout.Button("Key to Text")) KeyToText();
                }

                //save_load
                EditorGUILayout.Space();
            }
            if (EditorGUI.EndChangeCheck())
            {
                save();
            }

        }

        void add()
        {
            languagedictionary.languageContent[(int)Selected].Entitys.Add(new LanguageDictionaryEntity("", ""));
            bool[] Old = OpenField;
            for (int i = 0; i < OpenField.Length; i++)
            {
                if (i < Old.Length)
                {
                    OpenField[i] = Old[i];
                }
            }
        }


        void del(int i)
        {
            languagedictionary.languageContent[(int)Selected].Entitys.RemoveAt(i);
            bool[] Old = OpenField;
            for (int e = 0; e < OpenField.Length; e++)
            {
                if (e < i)
                {
                    OpenField[e] = Old[e];
                }
                else
                {
                    if ((e + 1) < OpenField.Length)
                    {
                        OpenField[e] = Old[e + 1];
                    }
                }
            }
        }

        void save()
        {
            languagedictionary.defaultLanguage = Default;
            string dataAsJson = JsonUtility.ToJson(languagedictionary, true);
            string filePath = Application.dataPath + "/Resources/" + Filename + ".json";
            try
            {
                File.WriteAllText(filePath, dataAsJson);
            }
            catch
            {
                this.ShowNotification(new GUIContent("Error"));
            }



        }


        void load()
        {
            string filePath = Application.dataPath + "/Resources/" + Filename + ".json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                languagedictionary = JsonUtility.FromJson<LanguageDictionary>(dataAsJson);
            }
            else
            {
                this.ShowNotification(new GUIContent("file not Found"));
                languagedictionary = new LanguageDictionary();
            }
        }

        void CopyText()
        {
            EditorJsonUtility.FromJsonOverwrite(EditorJsonUtility.ToJson(languagedictionary.languageContent[(int)FromLanguage]), languagedictionary.languageContent[(int)ToLanguage]);
        }

        void KeyToText()
        {
            for (int i = 0; i < languagedictionary.languageContent[(int)Selected].Entitys.Count; i++)
            {
                languagedictionary.languageContent[(int)Selected].Entitys[i].Text = languagedictionary.languageContent[(int)Selected].Entitys[i].Key;
            }
        }

        bool Translating;
        void TranslateText()
        {
            if (!Translating)
            {
                Translating = true;
                EditorCoroutine.start(Translate());
            }
        }

        void TranslateText(int i)
        {
            if (!Translating)
            {
                Translating = true;
                EditorCoroutine.start(Translate(i));
            }
        }

        void CopyAndTranslateAllText()
        {
            if (!Translating)
            {
                Translating = true;
                EditorCoroutine.start(TranslateAll());
            }
        }

        string TranslateTitle;
        string TranslateInfo;
        float progress;
        float progressTotal;

        IEnumerator TranslateAll()
        {
            yield return new WaitForFixedUpdate();
            for (int l = 0; l < 43; l++)
            {
                ToLanguage = (SystemLanguage)l;
                
                if (!ToNotTranslete[l] && (SystemLanguage)l != Default && (SystemLanguage)l != SystemLanguage.Unknown) {
                    CopyText();
                    TranslateTitle = "From " + FromLanguage + " To " + ToLanguage;
                    TranslateInfo = "";
                    progressTotal = languagedictionary.languageContent[(int)ToLanguage].Entitys.Count - 1;
                    for (int i = 0; i < languagedictionary.languageContent[(int)ToLanguage].Entitys.Count; i++)
                    {
                        progress = i;
                        string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
            + SystemLanguageToGoogleLanguage(FromLanguage) + "&tl=" + SystemLanguageToGoogleLanguage(ToLanguage) + "&dt=t&q=" + WWW.EscapeURL(languagedictionary.languageContent[(int)ToLanguage].Entitys[i].Text);

                        WWW www = new WWW(url);
                        while (!www.isDone)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                        if (string.IsNullOrEmpty(www.error))
                        {
                            var N = JSONNode.Parse(www.text);
                            if (!www.text.Contains("[null,null,"))
                            {
                                languagedictionary.languageContent[(int)ToLanguage].Entitys[i].Text = N[0][0][0];
                                TranslateInfo = Mathf.FloorToInt((progress / progressTotal) * 100).ToString() + "% " + N[0][0][0].ToString();
                            }
                            else
                            {
                                Debug.LogError("null");
                            }
                        }
                        else
                        {
                            Debug.LogError(www.error);
                        }
                        EditorUtility.DisplayProgressBar(TranslateTitle, TranslateInfo, progress / progressTotal);
                    }
                    EditorUtility.ClearProgressBar();
                    save();
                }
            }
            Translating = false;
        }

        IEnumerator Translate()
        {
            yield return new WaitForFixedUpdate();
            TranslateTitle = "From " + FromLanguage + " To " + ToLanguage;
            TranslateInfo = "";
            progressTotal = languagedictionary.languageContent[(int)ToLanguage].Entitys.Count - 1;
            for (int i = 0; i < languagedictionary.languageContent[(int)ToLanguage].Entitys.Count; i++)
            {
                progress = i;
                string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
    + SystemLanguageToGoogleLanguage(FromLanguage) + "&tl=" + SystemLanguageToGoogleLanguage(ToLanguage) + "&dt=t&q=" + WWW.EscapeURL(languagedictionary.languageContent[(int)ToLanguage].Entitys[i].Text);

                WWW www = new WWW(url);
                while (!www.isDone)
                {
                    yield return new WaitForFixedUpdate();
                }
                if (string.IsNullOrEmpty(www.error))
                {
                    var N = JSONNode.Parse(www.text);
                    if (!www.text.Contains("[null,null,"))
                    {
                        languagedictionary.languageContent[(int)ToLanguage].Entitys[i].Text = N[0][0][0];
                        TranslateInfo = Mathf.FloorToInt((progress / progressTotal) * 100).ToString() + "% " + N[0][0][0].ToString();
                    }
                    else
                    {
                        Debug.LogError("null");
                    }
                }
                else
                {
                    Debug.LogError(www.error);
                }
                EditorUtility.DisplayProgressBar(TranslateTitle, TranslateInfo, progress / progressTotal);
            }
            EditorUtility.ClearProgressBar();
            save();
            Translating = false;
        }

        IEnumerator Translate(int i)
        {
            yield return new WaitForFixedUpdate();
            TranslateTitle = "From " + FromLanguage + " To " + ToLanguage;
            TranslateInfo = "";
            EditorUtility.DisplayProgressBar(TranslateTitle, TranslateInfo, 0);
            string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
    + SystemLanguageToGoogleLanguage(FromLanguage) + "&tl=" + SystemLanguageToGoogleLanguage(ToLanguage) + "&dt=t&q=" + WWW.EscapeURL(languagedictionary.languageContent[(int)ToLanguage].Entitys[i].Text);

            WWW www = new WWW(url);
            while (!www.isDone)
            {
                yield return new WaitForFixedUpdate();
            }
            if (string.IsNullOrEmpty(www.error))
            {
                var N = JSONNode.Parse(www.text);
                if (!www.text.Contains("[null,null,"))
                {
                    languagedictionary.languageContent[(int)ToLanguage].Entitys[i].Text = N[0][0][0];
                    TranslateInfo = Mathf.FloorToInt((progress / progressTotal) * 100).ToString() + "% " + N[0][0][0].ToString();
                }
                else
                {
                    Debug.LogError("null?");
                }
            }
            else
            {
                Debug.LogError(www.error);
            }
            EditorUtility.DisplayProgressBar(TranslateTitle, TranslateInfo, 1);
            save();
            EditorUtility.ClearProgressBar();
            Translating = false;
        }

        public string SystemLanguageToGoogleLanguage(SystemLanguage Lang)
        {
            switch (Lang)
            {
                case SystemLanguage.Afrikaans: return "af";
                case SystemLanguage.Arabic: return "sq";
                case SystemLanguage.Basque: return "eu";
                case SystemLanguage.Belarusian: return "be";
                case SystemLanguage.Bulgarian: return "bg";
                case SystemLanguage.Catalan: return "ca";
                case SystemLanguage.Chinese: return "zh";
                case SystemLanguage.Czech: return "cs";
                case SystemLanguage.Danish: return "da";
                case SystemLanguage.Dutch: return "nl";
                case SystemLanguage.English: return "en";
                case SystemLanguage.Estonian: return "et";
                case SystemLanguage.Faroese: return "nn";//is "fo" but the Google translate don't have it
                case SystemLanguage.Finnish: return "fi";
                case SystemLanguage.French: return "fr";
                case SystemLanguage.German: return "de";
                case SystemLanguage.Greek: return "el";
                case SystemLanguage.Hebrew: return "he";
                case SystemLanguage.Hungarian: return "hu";
                case SystemLanguage.Icelandic: return "is";
                case SystemLanguage.Indonesian: return "id";
                case SystemLanguage.Italian: return "it";
                case SystemLanguage.Japanese: return "ja";
                case SystemLanguage.Korean: return "ko";
                case SystemLanguage.Latvian: return "lv";
                case SystemLanguage.Lithuanian: return "lt";
                case SystemLanguage.Norwegian: return "nb";
                case SystemLanguage.Polish: return "pl";
                case SystemLanguage.Portuguese: return "pt";
                case SystemLanguage.Romanian: return "ro";
                case SystemLanguage.Russian: return "ru";
                case SystemLanguage.SerboCroatian: return "hr";
                case SystemLanguage.Slovak: return "sk";
                case SystemLanguage.Slovenian: return "sl";
                case SystemLanguage.Spanish: return "es";
                case SystemLanguage.Swedish: return "sv";
                case SystemLanguage.Thai: return "th";
                case SystemLanguage.Turkish: return "tr";
                case SystemLanguage.Ukrainian: return "uk";
                case SystemLanguage.Vietnamese: return "vi";
                case SystemLanguage.ChineseSimplified: return "zh";
                case SystemLanguage.ChineseTraditional: return "zh";
                case SystemLanguage.Unknown: return "auto";
            }

            return "auto";
        }

    }
}
#endif