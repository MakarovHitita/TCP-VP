using System;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public delegate void OnNotifyDaltonismChangesHandler(DaltonismType type);
public delegate void OnNotifyLanguageChangesHandler(string language);

public enum DaltonismType
{
    NoDaltonism,
    Protanopia,
    Deuteranopia,
    Tritanopia,
}

[Serializable]
public class ConfigOptions
{
    [SerializeField] private DaltonismType _daltonismType;
    public DaltonismType DaltonismType { get => _daltonismType; set => _daltonismType = value; }
    [SerializeField] private string _language;
    public string Language { get => string.IsNullOrEmpty(_language) ? "en" : _language; set => _language = value; }
    public bool IsLanguageDefined {  get => !string.IsNullOrEmpty(_language); }

    public ConfigOptions()
    {
        _daltonismType = DaltonismType.NoDaltonism;
        _language = "";
    }
}

public class ConfigBehaviour : MonoBehaviour, ISceneCanvas, ISceneUI
{
    public static ConfigBehaviour Singleton { get; private set; }

    [SerializeField] private TMP_Text _configConsole;
    [SerializeField] private GameObject _configUI;
    
    [SerializeField] private TMP_Dropdown _languageSelectionDD;
   
    [SerializeField] private TMP_Dropdown _daltonismDD;
    [SerializeField] private TMP_Dropdown _languageDD;

    private event OnNotifyDaltonismChangesHandler OnDaltonismChangesEvent;
    private event OnNotifyLanguageChangesHandler OnLanguageChangesEvent;

    private event Action OnUpdate;

    public ConfigOptions Options { get; private set; }

    private string _configPath;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;

            _configPath = Path.Combine(Application.persistentDataPath, "Config", "config.json");
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Config"));
            if (File.Exists(_configPath))
            {
                try
                {
                    var json = File.ReadAllText(_configPath);
                    Options = JsonUtility.FromJson<ConfigOptions>(json);

                    if (Options == null)
                        throw new Exception("Config file invalid or incomplete.");
                }
                catch
                {
                    Debug.LogWarning("El archivo de configuración es inválido. Regenerando...");
                    RegenerateConfigFile();
                }
            }
            else
            {
                RegenerateConfigFile();
            }

        }
        else
            Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RegenerateConfigFile()
    {
        Options = new();
        File.WriteAllText(_configPath, JsonUtility.ToJson(Options, true));
    }

    public void AddOnNotifyDaltonismChangesEvent(OnNotifyDaltonismChangesHandler handler) => OnDaltonismChangesEvent += handler;
    public void RemoveOnNotifyDaltonismChangesEvent(OnNotifyDaltonismChangesHandler handler) => OnDaltonismChangesEvent -= handler;
    public void AddOnNotifyLanguageChnagesEvent(OnNotifyLanguageChangesHandler handler) => OnLanguageChangesEvent += handler;
    public void RemoveOnNotifyLanguageChangesEvent(OnNotifyLanguageChangesHandler handler) => OnLanguageChangesEvent -= handler;

    public void OnDaltonismValueChanged(int _)
    {
        OnUpdate += NotifyDaltonismChanges;
    }

    private void NotifyDaltonismChanges()
    {
        var type = (DaltonismType)_daltonismDD.value;
        Options.DaltonismType = type;
        OnDaltonismChangesEvent?.Invoke(type);
    }

    public void OnLanguageValueChanged(int _)
    {        
        OnUpdate += NotifyLanguageChanges;
    }

    private void NotifyLanguageChanges()
    {
        var option = _languageDD.value;
        var language = _languageDD.options[option].text.ToLower()[..2];
        Options.Language = language;
        OnLanguageChangesEvent?.Invoke(language);
    }

    public void OnDisclaimerLanguageValueChanged()
    {
        OnUpdate += NotifyDisclaimerLanguageChanges;
    }

    private void NotifyDisclaimerLanguageChanges()
    {
        var option = _languageSelectionDD.value;
        var language = _languageSelectionDD.options[option].text.ToLower()[..2];
        Options.Language = language;
        OnLanguageChangesEvent?.Invoke(language);
    }

    public void UpdateOptions()
    {
        OnUpdate?.Invoke();
        File.Delete(_configPath);
        File.WriteAllText(_configPath, JsonUtility.ToJson(Options, true));
        OnUpdate = null;
    }

    public void DiscardChanges()
    {
        OnUpdate = null;
        _daltonismDD.value = (int)Options.DaltonismType;
        _languageDD.value = LanguageManager.Singleton.LanguageIndex[Options.Language];
    }

    private void OnDestroy()
    {
        Singleton = null;
    }

    public void RestartConsoles()
    {
        _configConsole.text = "";
    }

    public void SetActive(bool active)
    {
        _configUI.SetActive(active);
    }
}
