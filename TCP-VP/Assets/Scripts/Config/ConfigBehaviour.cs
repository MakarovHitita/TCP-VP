using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum DaltonismType
{
    NoDaltonism,
    Protanopia,
    Deuteranopia,
    Tritanopia,
}

public enum GraphicsQuality
{
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh,
    Extreme,
}

public enum TextSize
{
    Small,
    Medium,
    Large,
}

[Serializable]
public class ConfigOptions
{
    [SerializeField] private DaltonismType _daltonismType;
    public DaltonismType DaltonismType { get => _daltonismType; set => _daltonismType = value; }
    [SerializeField] private string _language;
    public string Language { get => string.IsNullOrEmpty(_language) ? "en" : _language; set => _language = value; }
    public bool IsLanguageDefined {  get => !string.IsNullOrEmpty(_language); }
    [SerializeField] private float _masterSound;
    public float MasterSound { get => _masterSound; set => _masterSound = value; }
    [SerializeField] private float _musicSound;
    public float MusicSound { get => _musicSound; set => _musicSound = value; }
    [SerializeField] private float _effectsSound;
    public float EffectsSound { get => _effectsSound; set => _effectsSound = value; }
    [SerializeField] private float _chatSounds;
    public float ChatSound { get => _chatSounds; set => _chatSounds = value; }
    [SerializeField] private FullScreenMode _windowMode;
    public FullScreenMode WindowMode { get => _windowMode; set => _windowMode = value; }
    [SerializeField] private int _windowWidth;
    public int WindowWidth { get => _windowWidth; set => _windowWidth = value; }
    [SerializeField] private int _windowHeight;
    public int WindowHeight { get => _windowHeight; set => _windowHeight = value; }
    [SerializeField] private GraphicsQuality _graphicsQuality;
    public GraphicsQuality GraphicsQuality { get => _graphicsQuality; set => _graphicsQuality = value; }
    [SerializeField] private bool _effects;
    public bool Effects { get => _effects; set => _effects = value; }
    [SerializeField] private TextSize _textSize;
    public TextSize TextSize { get => _textSize; set => _textSize = value; }

    public ConfigOptions()
    {
        _daltonismType = DaltonismType.NoDaltonism;
        _language = "";
        _masterSound = 1f;
        _musicSound = 1f;
        _effectsSound = 1f;
        _chatSounds = 1f;
        _windowMode = FullScreenMode.FullScreenWindow;
        _windowWidth = 1920;
        _windowHeight = 1080;
        _effects = true;
        _textSize = TextSize.Medium;
    }
}

public class ConfigBehaviour : MonoBehaviour, ISceneCanvas, ISceneUI
{
    public static ConfigBehaviour Singleton { get; private set; }

    [SerializeField] private TMP_Text _configConsole;
    [SerializeField] private GameObject _configUI;
    [SerializeField] private GameObject _soundWindow;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _accesibility;
    [SerializeField] private GameObject _upButtons;
    [SerializeField] private GameObject _downButtons;

    [SerializeField] private int _transparencyPer100;
   
    [SerializeField] private Slider _masterSoundSlider;
    [SerializeField] private Slider _musicSoundSlider;
    [SerializeField] private Slider _effectsSoundSlider;
    [SerializeField] private Slider _chatSoundSlider;
    [SerializeField] private TMP_Dropdown _windowModeDD;
    [SerializeField] private TMP_InputField _windowWidthIF;
    [SerializeField] private TMP_InputField _windowHeightIF;
    [SerializeField] private TMP_Dropdown _graphicsDD;
    [SerializeField] private Toggle _effectsCheck;
    [SerializeField] private TMP_Dropdown _languageDD;
    [SerializeField] private TMP_Dropdown _daltonismDD;
    [SerializeField] private TMP_Dropdown _textSizeDD;

    private static event Action<float> OnMasterSoundChangesEvent;
    private static event Action<float> OnMusicSoundChangesEvent;
    private static event Action<float> OnEffectsSoundChangesEvent;
    private static event Action<float> OnChatSoundChangesEvent;
    private static event Action<GraphicsQuality> OnGraphicsChangesEvent;
    private static event Action<bool> OnShowEffectsChangesEvent;
    private static event Action<string> OnLanguageChangesEvent;
    private static event Action<DaltonismType> OnDaltonismChangesEvent;
    private static event Action<TextSize> OnTextSizeChangesEvent;

    private event Action OnUpdate;
    private HashSet<GameObject> UpdatingObjetcs { get; set; }

    public ConfigOptions Options { get; private set; }

    private string _configPath;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            UpdatingObjetcs = new();

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
            //RestartConsoles();
            InitElements();

            var texts = GetComponentsInChildren<LanguageInSelector>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].ChangeTransparency(_transparencyPer100);
            }
            var images = GetComponentsInChildren<Image>(true);
            Color color;
            for (int i = 0; i < images.Length; i++)
            {
                color = images[i].color;
                color.a = 100 / _transparencyPer100;
                images[i].color = color;
            }
            //color = Color.black;
            //color.a = 100 / _transparencyPer100;
            //_background.GetComponent<Image>().color = color;
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

    private void InitElements()
    {
        _upButtons.SetActive(true);
        _downButtons.SetActive(true);

        _masterSoundSlider.value = Options.MasterSound;
        _musicSoundSlider.value = Options.MusicSound;
        _effectsSoundSlider.value = Options.EffectsSound;
        _chatSoundSlider.value = Options.ChatSound;
        _windowModeDD.value = (int)Options.WindowMode;
        _windowWidthIF.text = Options.WindowWidth.ToString();
        _windowHeightIF.text = Options.WindowHeight.ToString();
        _graphicsDD.value = (int)Options.GraphicsQuality;
        _effectsCheck.isOn = Options.Effects;
        List<TMP_Dropdown.OptionData> options = new();
        var list = LanguageManager.Singleton.Languages;
        for (int i = 0; i < list.Count; i++)
            options.Add(new(list[i], SpriteManager.Singleton.SpriteDictionary[DaltonismType.NoDaltonism]["DropdownOption"], Color.white));
        _languageDD.AddOptions(options);
        _textSizeDD.value = (int)Options.TextSize;
    }

    public void OnSoundWindowButtonClick()
    {
        _soundWindow.SetActive(true);
        _game.SetActive(false);
        _accesibility.SetActive(false);
    }

    public void OnGameButtonClick()
    {
        _soundWindow.SetActive(false);
        _game.SetActive(true);
        _accesibility.SetActive(false);
    }

    public void OnAccesibilityButtonClick()
    {
        _soundWindow.SetActive(false);
        _game.SetActive(false);
        _accesibility.SetActive(true);
    }

    public static void AddOnNotifyMasterSoundChangesEvent(Action<float> handler) => OnMasterSoundChangesEvent += handler;
    public static void RemoveOnNotifyMasterSoundChangesEvent(Action<float> handler) => OnMasterSoundChangesEvent -= handler;
    public static void AddOnNotifyMusicSoundChangesEvent(Action<float> handler) => OnMusicSoundChangesEvent += handler;
    public static void RemoveOnNotifyMusicSoundChangesEvent(Action<float> handler) => OnMusicSoundChangesEvent -= handler;
    public static void AddOnNotifyEffectsSoundChangesEvent(Action<float> handler) => OnEffectsSoundChangesEvent += handler;
    public static void RemoveOnNotifyEffectsSoundChangesEvent(Action<float> handler) => OnEffectsSoundChangesEvent -= handler;
    public static void AddOnNotifyChatSoundChangesEvent(Action<float> handler) => OnChatSoundChangesEvent += handler;
    public static void RemoveOnNotifyChatSoundChangesEvent(Action<float> handler) => OnChatSoundChangesEvent -= handler;
    public static void AddOnNotifyLanguageChnagesEvent(Action<string> handler) => OnLanguageChangesEvent += handler;
    public static void RemoveOnNotifyLanguageChangesEvent(Action<string> handler) => OnLanguageChangesEvent -= handler;
    public static void AddOnNotifyDaltonismChangesEvent(Action<DaltonismType> handler) => OnDaltonismChangesEvent += handler;
    public static void RemoveOnNotifyDaltonismChangesEvent(Action<DaltonismType> handler) => OnDaltonismChangesEvent -= handler;
    public static void AddOnNotifyTextSizeChangesEvent(Action<TextSize> handler) => OnTextSizeChangesEvent += handler;
    public static void RemoveOnNotifyTextSizeChangesEvent(Action<TextSize> handler) => OnTextSizeChangesEvent -= handler;

    public void OnMasterSoundValueChanged(Single _)
    {
        AddOnUpdate(_masterSoundSlider.gameObject, NotityMasterSoundValueChanged);
    }

    private void NotityMasterSoundValueChanged()
    {
        var volume = _masterSoundSlider.value;
        Options.MasterSound = volume;
        OnMasterSoundChangesEvent?.Invoke(volume);
    }

    public void OnMusicSoundValueChanged(Single _)
    {
        AddOnUpdate(_musicSoundSlider.gameObject, NotityMusicSoundValueChanged);
    }

    private void NotityMusicSoundValueChanged()
    {
        var volume = _musicSoundSlider.value;
        Options.MusicSound = volume;
        OnMusicSoundChangesEvent?.Invoke(volume);
    }

    public void OnEffectsSoundValueChanged(Single _)
    {
        AddOnUpdate(_effectsSoundSlider.gameObject, NotityEffectsSoundValueChanged);
    }

    private void NotityEffectsSoundValueChanged()
    {
        var volume = _effectsSoundSlider.value;
        Options.EffectsSound = volume;
        OnEffectsSoundChangesEvent?.Invoke(volume);
    }

    public void OnChatSoundValueChanged(Single _)
    {
        AddOnUpdate(_chatSoundSlider.gameObject, NotityChatSoundValueChanged);
    }

    private void NotityChatSoundValueChanged()
    {
        var volume = _chatSoundSlider.value;
        Options.ChatSound = volume;
        OnChatSoundChangesEvent?.Invoke(volume);
    }

    public void OnWindowModeValueChanged(int _)
    {
        AddOnUpdate(_windowModeDD.gameObject, NotityWindowModeValueChanged);
    }

    private void NotityWindowModeValueChanged()
    {
        var mode = _windowModeDD.value;
        Options.WindowMode = (FullScreenMode)mode;
        Screen.fullScreenMode = (FullScreenMode)mode;
    }

    public void OnWindowWidthValueChanged(string _)
    {
        AddOnUpdate(_windowWidthIF.gameObject, NotityWindowWidthValueChanged);
    }

    private void NotityWindowWidthValueChanged()
    {
        if (int.TryParse(_windowWidthIF.text, out var width))
        {
            Options.WindowWidth = width;
            Screen.SetResolution(width, Options.WindowHeight, Options.WindowMode);
        }
        else
            _windowWidthIF.text = Options.WindowWidth.ToString();
    }

    public void OnWindowHeightValueChanged(string _)
    {
        AddOnUpdate(_windowHeightIF.gameObject, NotityWindowHeightValueChanged);
    }

    private void NotityWindowHeightValueChanged()
    {
        if (int.TryParse(_windowHeightIF.text, out var height))
        {
            Options.WindowHeight = height;
            Screen.SetResolution(Options.WindowWidth, height, Options.WindowMode);
        }
        else
            _windowHeightIF.text = Options.WindowHeight.ToString();
    }

    public void OnGraphicsValueChanged(int _)
    {
        AddOnUpdate(_graphicsDD.gameObject, NotityGraphicsValueChanged);
    }

    private void NotityGraphicsValueChanged()
    {
        var quality = (GraphicsQuality)_graphicsDD.value;
        Options.GraphicsQuality = quality;
        OnGraphicsChangesEvent?.Invoke(quality);
    }

    public void OnEffectsValueChanged(bool _)
    {
        AddOnUpdate(_effectsCheck.gameObject, NotityEffectsValueChanged);
    }

    private void NotityEffectsValueChanged()
    {
        var effects = _effectsCheck.isOn;
        Options.Effects = effects;
        OnShowEffectsChangesEvent?.Invoke(effects);
    }
    public void OnDaltonismValueChanged(int _)
    {
        AddOnUpdate(_daltonismDD.gameObject, NotifyDaltonismChanges);
    }

    private void NotifyDaltonismChanges()
    {
        var type = (DaltonismType)_daltonismDD.value;
        Options.DaltonismType = type;
        OnDaltonismChangesEvent?.Invoke(type);
    }


    public void OnTextSizeValueChanged(int _)
    {
        AddOnUpdate(_textSizeDD.gameObject, NotifyTextSizeChanges);
    }

    private void NotifyTextSizeChanges()
    {
        var size = (TextSize)_textSizeDD.value;
        Options.TextSize = size;
        OnTextSizeChangesEvent?.Invoke(size);
    }

    public void OnLanguageValueChanged(int _)
    {
        AddOnUpdate(_languageDD.gameObject, NotifyLanguageChanges);
    }

    private void NotifyLanguageChanges()
    {
        var option = _languageDD.value;
        var language = _languageDD.options[option].text.ToLower()[..2];
        Options.Language = language;
        OnLanguageChangesEvent?.Invoke(language);
    }

    public void OnDisclaimerLanguageValueChanged(string language)
    {
        Options.Language = language;
        OnLanguageChangesEvent?.Invoke(language);
        UpdateOptions();
    }

    private void AddOnUpdate(GameObject @object, Action action)
    {
        if (UpdatingObjetcs.Contains(@object))
            return;
        UpdatingObjetcs.Add(@object);
        OnUpdate += action;
    }

    public void UpdateOptions()
    {
        OnUpdate?.Invoke();
        UpdatingObjetcs.Clear();
        File.Delete(_configPath);
        File.WriteAllText(_configPath, JsonUtility.ToJson(Options, true));
        OnUpdate = null;
    }

    public void DiscardChanges()
    {
        OnUpdate = null;
        UpdatingObjetcs.Clear();
        InitElements();
    }

    public void GoBack()
    {
        DiscardChanges();
        CanvasScene.Singleton.ChangeScene((Scene)SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        Singleton = null;
    }

    public void RestartConsoles()
    {
        _soundWindow.SetActive(true);
        _game.SetActive(false);
        _accesibility.SetActive(false);
        return;
    }

    public void SetActive(bool active)
    {
        _configUI.SetActive(active);
    }
}
