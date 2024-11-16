using System;
using System.IO;
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
    public DaltonismType DaltonismType { get => _daltonismType; set { _daltonismType = value; OnUpdate?.Invoke(); } }
    [SerializeField] private string _language;
    public string Language { get => _language; set { _language = value; OnUpdate?.Invoke(); } }

    private event Action OnUpdate;

    public ConfigOptions(Action onUpdate)
    {
        OnUpdate = onUpdate;
        _daltonismType = DaltonismType.NoDaltonism;
        _language = "en";
    }
}

public class ConfigBehaviour : MonoBehaviour
{
    public static ConfigBehaviour Singleton { get; private set; }

    [SerializeField] private TMP_Dropdown _languageDD;

    private event OnNotifyDaltonismChangesHandler OnDaltonismChangesEvent;
    private event OnNotifyLanguageChangesHandler OnLanguageChangesEvent;

    public ConfigOptions Options { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;

            var path = Path.Combine(Application.persistentDataPath, "Config", "config.json");
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Config"));
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    Options = JsonUtility.FromJson<ConfigOptions>(json);

                    if (Options == null)
                        throw new Exception("Config file invalid or incomplete.");
                }
                catch
                {
                    Debug.LogWarning("El archivo de configuraci�n es inv�lido. Regenerando...");
                    RegenerateConfigFile(path);
                }
            }
            else
            {
                RegenerateConfigFile(path);
            }

        }
        else
        {
            Singleton = null;
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RegenerateConfigFile(string configPath)
    {
        Options = new(UpdateOptions);
        File.WriteAllText(configPath, JsonUtility.ToJson(Options, true));
    }

    public void AddOnNotifyDaltonismChangesEvent(OnNotifyDaltonismChangesHandler handler) => OnDaltonismChangesEvent += handler;
    public void RemoveOnNotifyDaltonismChangesEvent(OnNotifyDaltonismChangesHandler handler) => OnDaltonismChangesEvent -= handler;
    public void AddOnNotifyLanguageChnagesEvent(OnNotifyLanguageChangesHandler handler) => OnLanguageChangesEvent += handler;
    public void RemoveOnNotifyLnaguageChangesEvent(OnNotifyLanguageChangesHandler handler) => OnLanguageChangesEvent -= handler;

    public void NotifyDaltonismChanges(int type)
    {
        Options.DaltonismType = (DaltonismType)type;
        OnDaltonismChangesEvent?.Invoke((DaltonismType)type);
    }

    public void NotifyLanguageChanges(int option)
    {
        var language = _languageDD.options[option].text.ToLower()[..2];
        Options.Language = language;
        OnLanguageChangesEvent?.Invoke(language);
    }

    private void UpdateOptions()
    {

    }

    private void OnDestroy()
    {
        Singleton = null;
    }
}
