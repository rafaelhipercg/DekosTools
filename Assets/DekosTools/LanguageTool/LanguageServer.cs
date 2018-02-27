using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DekosTools.Language
{
    public static class LanguageServer
    {
        static SystemLanguage defaultLanguage = SystemLanguage.English;

        static SystemLanguage languageUsed = SystemLanguage.Unknown;

        static LanguageDictionary dictionary;

        static void AutoSelect() {
            if (Application.systemLanguage != SystemLanguage.Unknown)
            {
                languageUsed = Application.systemLanguage;
            }
            else {
                languageUsed = defaultLanguage;
            }

        }

        static public void Initialize(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown) {
                AutoSelect();
            }
        }

        static public string GetText(string key) {
            Dictionary<string, string> Tmpdictionary;
            dictionary.Texts.TryGetValue(languageUsed, out Tmpdictionary);
            string Output;
            Tmpdictionary.TryGetValue(key, out Output);
            return Output;
        }

        static public Sprite GetSprite(string key)
        {
            Dictionary<string, Sprite> Tmpdictionary;
            dictionary.Sprite.TryGetValue(languageUsed, out Tmpdictionary);
            Sprite Output;
            Tmpdictionary.TryGetValue(key, out Output);
            return Output;
        }

        static public Texture GetTexture(string key)
        {
            Dictionary<string, Texture> Tmpdictionary;
            dictionary.Texture.TryGetValue(languageUsed, out Tmpdictionary);
            Texture Output;
            Tmpdictionary.TryGetValue(key, out Output);
            return Output;
        }

        static public AudioClip GetAudioClip(string key)
        {
            Dictionary<string, AudioClip> Tmpdictionary;
            dictionary.Audio.TryGetValue(languageUsed, out Tmpdictionary);
            AudioClip Output;
            Tmpdictionary.TryGetValue(key, out Output);
            return Output;
        }
    }
}
