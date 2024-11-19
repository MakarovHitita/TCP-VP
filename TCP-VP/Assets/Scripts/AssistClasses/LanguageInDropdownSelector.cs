using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LanguageInDropdownSelector : LanguageInSelector
{
    private List<string> Texts { get; set; }
    private TMP_Dropdown Dropdown { get; set; }

    protected override void Init()
    {
        Name = _objectReference.name;
        Texts = new();
        Dropdown = GetComponent<TMP_Dropdown>();
        List<string> texts = new();
        var wholeTexts = LanguageManager.Singleton.LanguageTexts[ConfigBehaviour.Singleton.Options.Language].Keys.ToList();
        for (int i = 0; i < wholeTexts.Count; i++)
        {
            var name = wholeTexts[i].Split('_', System.StringSplitOptions.RemoveEmptyEntries);
            if (name[0] == Name)
                texts.Insert(int.Parse(name[1]), wholeTexts[i]);
        }
        List<TMP_Dropdown.OptionData> options = new();
        for (int i = 0; i < texts.Count; i++)
        {
            options.Add(new(LanguageManager.Singleton.LanguageTexts[ConfigBehaviour.Singleton.Options.Language][texts[i]]));
            Texts.Add(texts[i]);
        }
        Dropdown.AddOptions(options);
    }

    protected override void ChangeLanguageText(string language)
    {
        for (int i = 0; i < Texts.Count; i++)
        {
            Dropdown.options[i].text = LanguageManager.Singleton.LanguageTexts[language][Texts[i]];
        }
    }

    public override void ChangeTransparency(int tranparecyPer100)
    {
        if (Texts == null)
        {
            Init();
        }
        Color color;
        for (int i = 0; i < Texts.Count; i++)
        {
            color = Dropdown.options[i].color;
            color.a = 100 / tranparecyPer100;
            Dropdown.options[i].color = color;
        }
    }

    protected override void ChangeTextSize(TextSize size)
    {
        var texts = GetComponentsInChildren<TMP_Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].fontSize = 10 + (6 * (int)size);
        }
    }
}
