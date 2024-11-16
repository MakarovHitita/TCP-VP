using TMPro;
using UnityEngine;

public class LanguageInObjectSelector : MonoBehaviour
{
    [SerializeField] private GameObject _objectReference;
    private TMP_Text Text { get; set; }
    private string Name { get; set; }

    private void Awake()
    {
        Name = _objectReference.name;
        Text = GetComponent<TMP_Text>();
        ConfigBehaviour.Singleton.AddOnNotifyLanguageChnagesEvent(ChangeLanguageText);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeLanguageText(string language)
    {
        Text.text = LanguageManager.Singleton.LanguageTexts[language][Name];
    }
}
