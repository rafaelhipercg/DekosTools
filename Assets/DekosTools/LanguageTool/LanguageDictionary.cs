using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DekosTools.Language
{

    [System.Serializable]
    public class LanguageDictionary
    {
        [SerializeField]
        public Dictionary<SystemLanguage, Dictionary<string, string>> Texts;
        [SerializeField]
        public Dictionary<SystemLanguage, Dictionary<string, Sprite>> Sprite;
        [SerializeField]
        public Dictionary<SystemLanguage, Dictionary<string, Texture>> Texture;
        [SerializeField]
        public Dictionary<SystemLanguage, Dictionary<string, AudioClip>> Audio;
    }
}
