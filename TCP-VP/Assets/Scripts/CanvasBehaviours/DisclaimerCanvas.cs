using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisclaimerCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text _languageSelectionConsole;
    [SerializeField] private TMP_Text _languageSelectionDisclaimer;
    [SerializeField] private GameObject _languageSelection;
    [SerializeField] private TMP_Dropdown _languageSelectionDD;
    [SerializeField] private GameObject _languagesSelectionB;

    [SerializeField] private List<TextAsset> _languageDisclaimers;
    [SerializeField] private TextAsset _disclaimerConsole;

    private string StartConsole { get; set; }
    private string SubmitConsole { get; set; }

    [SerializeField] private float _disclaimerTimePerChar;
    private Dictionary<string, string> Disclaimers { get; set; }

    private void Start()
    {
        var orders = _disclaimerConsole.text.Split(":\t", StringSplitOptions.RemoveEmptyEntries);

        StartConsole = orders[0];
        SubmitConsole = orders[1];

        Disclaimers = new();
        for (int i = 0; i < _languageDisclaimers.Count; i++)
        {
            Disclaimers.Add(_languageDisclaimers[i].name[..2], _languageDisclaimers[i].text);
        }

        if (string.IsNullOrEmpty(ConfigBehaviour.Singleton.Options.Language))
        {
            StartCoroutine(nameof(ShowLanguageSelector));
        }
        else
            Submit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Submit()
    {
        _languageSelection.SetActive(false);
        ConfigBehaviour.Singleton.UpdateOptions();
        StartCoroutine(nameof(ShowDisclaimer));
    }

    private IEnumerator ShowLanguageSelector()
    {
        for (int i = 0; i < StartConsole.Length; i++)
        {
            char c = StartConsole[i];
            float speed = _disclaimerTimePerChar;
            if (c.ToString() == Environment.NewLine)
                speed *= 100;
            _languageSelectionConsole.text += c;
            yield return new WaitForSecondsRealtime(speed);
        }
        var languages = LanguageManager.Singleton.Languages;
        List<TMP_Dropdown.OptionData> optionDatas = new();
        for (int i = 0; i < languages.Count; i++)
        {
            optionDatas.Add(new(languages[i], SpriteManager.Singleton.SpriteDictionary[DaltonismType.NoDaltonism]["DropdownOption"], Color.white));
        }
        _languageSelectionDD.AddOptions(optionDatas);
        _languageSelectionDD.gameObject.SetActive(true);
        _languagesSelectionB.gameObject.SetActive(true);
    }
    
    private IEnumerator ShowDisclaimer()
    {
        var text = SubmitConsole;
        if (_languageSelectionConsole.IsActive())
            for (int i = -3; i < text.Length; i++)
            {
                char c;
                float speed = _disclaimerTimePerChar;
                if (i == -3 || i == -2)
                {
                    c = ConfigBehaviour.Singleton.Options.Language.ToLower()[i + 3];
                    speed *= 100;
                }
                else if (i == -1)
                    c = Environment.NewLine[0];
                else
                    c = text[i];
                _languageSelectionConsole.text += c;
                if (c.ToString() == Environment.NewLine)
                    speed *= 100;
                yield return new WaitForSecondsRealtime(speed);
            }
        yield return new WaitForSecondsRealtime(1.5f);
        _languageSelectionConsole.gameObject.SetActive(false);
        _languageSelectionDisclaimer.gameObject.SetActive(true);
        var disclaimer = Disclaimers[ConfigBehaviour.Singleton.Options.Language];
        for (int i = 0; i < disclaimer.Length; i++)
        {
            float speed = _disclaimerTimePerChar*10;
            if (disclaimer[i].ToString() == Environment.NewLine)
                speed *= 100;
            _languageSelectionDisclaimer.text += disclaimer[i];
            yield return new WaitForSecondsRealtime(speed);
        }
        yield return new WaitForSecondsRealtime(_disclaimerTimePerChar * 100000);
        CanvasScene.Singleton.ChangeScene(Scene.MainMenu);
    }
}
