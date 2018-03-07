using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DekosTools.LanguageTranslate
{

    [System.Serializable]
    public class LanguageDictionary
    {
        [SerializeField]
        public SystemLanguage defaultLanguage = SystemLanguage.English;
        [SerializeField]
        public LanguageDictionaryContent[] languageContent;

        public LanguageDictionary(){
            languageContent = new LanguageDictionaryContent[43];
        }

    }

    [System.Serializable]
    public class LanguageDictionaryContent
    {
        [SerializeField]
        public bool Enable = false;
        [SerializeField]
        public List<LanguageDictionaryEntity> Entitys;

        public LanguageDictionaryContent() {
            Entitys = new List<LanguageDictionaryEntity>();
        }
    }

    [System.Serializable]
    public class LanguageDictionaryEntity
    {
        [SerializeField]
        public string Key = "";
        [SerializeField]
        public string Text = "";

        public LanguageDictionaryEntity(string key,string text){
            Key = key;
            Text = text;
        }
    }
}
