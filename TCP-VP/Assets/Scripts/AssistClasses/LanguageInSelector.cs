using UnityEngine;

public abstract class LanguageInSelector : MonoBehaviour
{
    [SerializeField] protected GameObject _objectReference;
    protected string Name { get; set; }
    [SerializeField] protected bool _white;
    [SerializeField] protected bool _semiTransparent;
    [SerializeField] protected int _tranparencyPer100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_tranparencyPer100 == 0)
            _tranparencyPer100 = 100;
        ConfigBehaviour.AddOnNotifyLanguageChnagesEvent(ChangeLanguageText);
        ConfigBehaviour.AddOnNotifyTextSizeChangesEvent(ChangeTextSize);
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void Init();
    protected abstract void ChangeLanguageText(string language);
    public abstract void ChangeTransparency(int tranparecyPer100);
    protected abstract void ChangeTextSize(TextSize size);

    private void OnDestroy()
    {
       ConfigBehaviour.RemoveOnNotifyLanguageChangesEvent(ChangeLanguageText);
    }
}
