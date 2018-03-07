using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DekosTools.LanguageTranslate;

[RequireComponent(typeof(Text))]
public class TextTranslate : MonoBehaviour {
    [SerializeField]
    string key;

    private void OnEnable()
    {
        LanguageServer.onChange += Start;
    }

    private void OnDisable()
    {
        LanguageServer.onChange -= Start;
    }

    public void Start () {
       GetComponent<Text>().text = LanguageServer.GetText(key);
	}
}
