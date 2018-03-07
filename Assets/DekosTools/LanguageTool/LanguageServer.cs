using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace DekosTools.LanguageTranslate
{
    public static class LanguageServer
    {
        static SystemLanguage defaultLanguage = SystemLanguage.English;
        public static SystemLanguage DefaultLanguage
        {
            get
            {
                return defaultLanguage;
            }
        }

        static SystemLanguage languageUsed = SystemLanguage.Unknown;
        public static SystemLanguage LanguageUsed
        {
            get
            {
                return languageUsed;
            }
        }

        static LanguageDictionary languagedictionary;

        const string Filename = "Language";

        public delegate void OnChange();
        public static OnChange onChange;

        static void AutoSelect() {
            if (Application.systemLanguage != SystemLanguage.Unknown)
            {
                if (!languagedictionary.languageContent[(int)Application.systemLanguage].Enable)
                {
                    languageUsed = defaultLanguage;
                }
                else {
                    languageUsed = Application.systemLanguage;
                }
            }
            else {
                languageUsed = defaultLanguage;
            }

        }

        static public string GetText(string key) {
            return GetText(key, languageUsed);
        }

        static public string GetText(string key, SystemLanguage language)
        {
            if (!initialized) {
                Debug.LogWarning("LanguageServer not initialized. Please use LanguageServer.Initialize()");
                Initialize(language);
            }
            LanguageDictionaryEntity LD = new LanguageDictionaryEntity("","");
            if (languagedictionary.languageContent[(int)language].Enable)
            {
                LD = languagedictionary.languageContent[(int)language].Entitys.Find((i) => i.Key == key);
            }
            else {
                LD = languagedictionary.languageContent[(int)defaultLanguage].Entitys.Find((i) => i.Key == key);
            }
            return LD.Text;
        }

        static bool initialized;

        static public void Initialize(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (!initialized) {
                load();
                defaultLanguage = languagedictionary.defaultLanguage;
            }
            ChangeLanguage(language);
            initialized = true;
        }

        static public void ChangeLanguage(SystemLanguage language = SystemLanguage.Unknown) {
            if (language == SystemLanguage.Unknown)
            {
                AutoSelect();
            }
            else {
                if (languagedictionary.languageContent[(int)language].Enable)
                {
                    languageUsed = language;
                }
                else {
                    languageUsed = defaultLanguage;
                }
            }
            if (onChange != null)
            {
                onChange();
            }
        }

        static void load()
        {

           TextAsset LodedText = Resources.Load(Filename) as TextAsset;
            
            if (LodedText.text == "")
            {
                languagedictionary = new LanguageDictionary();
                Debug.LogError("LanguageTranslate: File not found");

            }
            else
            {
                languagedictionary = JsonUtility.FromJson<LanguageDictionary>(LodedText.text);
            }
        }
    }
}
