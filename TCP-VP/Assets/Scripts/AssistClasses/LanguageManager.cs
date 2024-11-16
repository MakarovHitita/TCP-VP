using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Singleton { get; private set; }
    [SerializeField] private List<TextAsset> _texts;

    public Dictionary<string, Dictionary<string, string>> LanguageTexts { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            LanguageTexts = new();
            for (int i = 0; i < _texts.Count; i++)
            {
                LanguageTexts.Add(_texts[i].name, new());
                var languageText = _texts[i].text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < languageText.Length; j++)
                {
                    var languageTextEntries = languageText[j].Split(":\t");
                    LanguageTexts[_texts[i].name].Add(languageTextEntries[0], languageTextEntries[^1]);
                }
            }
        }
        else
        {
            Singleton = null;
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
