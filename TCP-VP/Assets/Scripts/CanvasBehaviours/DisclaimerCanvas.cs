using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisclaimerCanvas : MonoBehaviour, ISceneCanvas, ISceneUI
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

        if (!ConfigBehaviour.Singleton.Options.IsLanguageDefined)
        {
            StartCoroutine(nameof(ShowLanguageSelector));
        }
        else
            SubmitNoLanguage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubmitNoLanguage()
    {
        Submit(false);
    }

    public void OnSubmitButtonClick()
    {
        Submit(true);
    }

    private void Submit(bool hasLanguageShown = true)
    {
        _languageSelection.SetActive(false);
        if (hasLanguageShown)
        {
            ConfigBehaviour.Singleton.OnDisclaimerLanguageValueChanged();
            ConfigBehaviour.Singleton.UpdateOptions();
        }
        StartCoroutine(ShowDisclaimer(hasLanguageShown));
    }

    private IEnumerator ShowLanguageSelector()
    {
        yield return new WaitForSecondsRealtime(1);
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
        yield return new WaitForSecondsRealtime(0.5f);
        _languageSelectionDD.gameObject.SetActive(true);
        _languagesSelectionB.gameObject.SetActive(true);
    }

    private IEnumerator ShowDisclaimer(bool hasLanguageShown)
    {
        var text = SubmitConsole;
        string language = ConfigBehaviour.Singleton.Options.Language.ToLower();
        if (hasLanguageShown)
        {
            for (int i = 0; i < language.Length; i++)
            {
                _languageSelectionConsole.text += language[i];
                yield return new WaitForSecondsRealtime(_disclaimerTimePerChar * 100);
            }
            yield return new WaitForSecondsRealtime(_disclaimerTimePerChar * 100);
        }
        float speed;
        for (int i = 0; i < text.Length; i++)
        {
            speed = _disclaimerTimePerChar;
            char c = text[i];
            _languageSelectionConsole.text += c;
            if (c == Environment.NewLine[0])
                speed *= 100;
            yield return new WaitForSecondsRealtime(speed);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        _languageSelectionConsole.gameObject.SetActive(false);
        _languageSelectionDisclaimer.gameObject.SetActive(true);
        var disclaimer = Disclaimers[ConfigBehaviour.Singleton.Options.Language];
        speed = _disclaimerTimePerChar * 10;
        for (int i = 0; i < disclaimer.Length; i++)
        {
            if (disclaimer[i].ToString() == Environment.NewLine)
                speed *= 100;
            _languageSelectionDisclaimer.text += disclaimer[i];
            yield return new WaitForSecondsRealtime(speed);
        }
        yield return new WaitForSecondsRealtime(_disclaimerTimePerChar * 100000);
        CanvasScene.Singleton.ChangeScene(Scene.MainMenu);
    }

    public void RestartConsoles()
    {
        _languageSelectionConsole.text = "";
        Start();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
