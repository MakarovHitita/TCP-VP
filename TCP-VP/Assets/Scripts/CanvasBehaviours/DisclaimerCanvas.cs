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
        var rutine = StartCoroutine("ShowDisclaimer");
    }

    //private IEnumerator ShowDisclaimer()
    //{
    //    var disclaimer = Disclaimers
    //    for (int i = 0, i < )
    //}
}
