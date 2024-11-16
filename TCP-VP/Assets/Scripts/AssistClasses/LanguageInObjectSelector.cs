using TMPro;
using UnityEngine;

public class LanguageInObjectSelector : MonoBehaviour
{
    [SerializeField] private GameObject _objectReference;
    private TMP_Text Text { get; set; }
    private string Name { get; set; }

    private void Start()
    {
        Name = _objectReference.name;
        Text = GetComponent<TMP_Text>();
        ConfigBehaviour.Singleton.AddOnNotifyLanguageChnagesEvent(ChangeLanguageText);
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
