using System;
using TMPro;
using UnityEngine;

public class LanguageInObjectSelector : MonoBehaviour
{
    [SerializeField] private GameObject _objectReference;
    private TMP_Text Text { get; set; }
    private string Name { get; set; }
    [SerializeField] private bool _white;

    private void Start()
    {
        try
        {
            Name = _objectReference.name;
            Text = GetComponent<TMP_Text>();
            Text.color = _white ? Color.white : Color.black;
            ConfigBehaviour.Singleton.AddOnNotifyLanguageChnagesEvent(ChangeLanguageText);
            Text.text = LanguageManager.Singleton.LanguageTexts[ConfigBehaviour.Singleton.Options.Language][Name];
        }
        catch (Exception ex)
        {
            Debug.Log(gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeLanguageText(string language)
    {
        Text.text = LanguageManager.Singleton.LanguageTexts[language][Name];
    }

    private void OnDestroy()
    {
        ConfigBehaviour.Singleton.RemoveOnNotifyLanguageChangesEvent(ChangeLanguageText);
    }
}
