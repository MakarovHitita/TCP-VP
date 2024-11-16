using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisclaimerCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _languageSelectionDD;

    private void Start()
    {
        var languages = LanguageManager.Singleton.Languages;
        List<TMP_Dropdown.OptionData> optionDatas = new();
        for (int i = 0; i < languages.Count; i++)
        {
            optionDatas.Add(new(languages[i], SpriteManager.Singleton.SpriteDictionary[DaltonismType.NoDaltonism]["DropdownOption"], Color.white));
        }
        _languageSelectionDD.AddOptions(optionDatas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
