using TMPro;
using UnityEngine;

public class LanguageInObjectSelector : LanguageInSelector
{
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Init()
    {
        Name = _objectReference.name;
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }
        var color = _white ? Color.white : Color.black;
        color.a = _semiTransparent ? 100 / _tranparencyPer100 : 1;
        _text.color = color;
        _text.text = LanguageManager.Singleton.LanguageTexts[ConfigBehaviour.Singleton.Options.Language][Name];
    }

    protected override void ChangeLanguageText(string language)
    {
        _text.text = LanguageManager.Singleton.LanguageTexts[language][Name];
    }

    public override void ChangeTransparency(int tranparecyPer100)
    {
        if (tranparecyPer100 == 0)
        {
            _semiTransparent = false;
            return;
        }
        _semiTransparent = true;
        var color = _text.color;
        color.a = 100 / tranparecyPer100;
        _text.color = color;  
    }

    protected override void ChangeTextSize(TextSize size)
    {
        _text.fontSize = 10 + (6 * (int)size);
    }
}
