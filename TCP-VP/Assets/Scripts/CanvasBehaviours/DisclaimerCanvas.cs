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

    [SerializeField] private List<TextAsset> _languageDisclaimers;
    [SerializeField] private TextAsset _consoleSubmit;

    [SerializeField] private float _disclaimerSpeed;
    private Dictionary<string, string> Disclaimers { get; set; }

    private void Start()
    {
        Disclaimers = new();
        for (int i = 0; i < _languageDisclaimers.Count; i++)
        {
            Disclaimers.Add(_languageDisclaimers[i].name[..2], _languageDisclaimers[i].text);
        }

        if (string.IsNullOrEmpty(ConfigBehaviour.Singleton.Options.Language))
        {
            var languages = LanguageManager.Singleton.Languages;
            List<TMP_Dropdown.OptionData> optionDatas = new();
            for (int i = 0; i < languages.Count; i++)
            {
                optionDatas.Add(new(languages[i], SpriteManager.Singleton.SpriteDictionary[DaltonismType.NoDaltonism]["DropdownOption"], Color.white));
            }
            _languageSelectionDD.AddOptions(optionDatas);
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

    private IEnumerator ShowDisclaimer()
    {
        var text = _consoleSubmit.text;
        if (_languageSelectionConsole.IsActive())
            for (int i = -2; i < text.Length; i++)
            {
                char c;
                if (i == -2 || i == -1)
                    c = ConfigBehaviour.Singleton.Options.Language.ToLower()[i + 2];
                else
                    c = text[i];
                _languageSelectionConsole.text += c;
                float speed = _disclaimerSpeed / 100;
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
            float speed = _disclaimerSpeed / 10;
            if (disclaimer[i].ToString() == Environment.NewLine)
                speed *= 100;
            _languageSelectionDisclaimer.text += disclaimer[i];
            yield return new WaitForSecondsRealtime(speed);
        }
        yield return new WaitForSecondsRealtime(_disclaimerSpeed * 100);
        CanvasScene.Singleton.ChangeScene(Scene.MainMenu);
    }
}
