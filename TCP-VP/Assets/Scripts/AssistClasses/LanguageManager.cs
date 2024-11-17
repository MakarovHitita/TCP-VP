using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Singleton { get; private set; }
    [SerializeField] private List<TextAsset> _texts;

    public List<string> Languages { get; private set; }
    public Dictionary<string, int> LanguageIndex { get; private set; }
    public Dictionary<string, Dictionary<string, string>> LanguageTexts { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            LanguageTexts = new();
            Languages = new();
            LanguageIndex = new();
            for (int i = 0; i < _texts.Count; i++)
            {
                LanguageTexts.Add(_texts[i].name, new());
                LanguageIndex.Add(_texts[i].name, i);
                var languageText = _texts[i].text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                Languages.Add(languageText[0]);
                for (int j = 1; j < languageText.Length; j++)
                {
                    var languageTextEntries = languageText[j].Split(":\t");
                    LanguageTexts[_texts[i].name].Add(languageTextEntries[0], languageTextEntries[^1]);
                }
            }
        }
        else        
            Destroy(this);        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Singleton = null;
    }
}
