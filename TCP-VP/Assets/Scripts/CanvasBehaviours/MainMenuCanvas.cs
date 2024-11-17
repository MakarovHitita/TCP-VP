using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MainMenuActionButtons
{
    None = 0,
    Start,
    Game,
    Config,
    Credits,
    Tutorial,
    Single,
    Multi,
    CreateLobby,
    JoinLobby,
    FastMatch,
    CancelModules,
    ConnectLobby,
    Exit,
}

public class MainMenuCanvas : MonoBehaviour, ISceneCanvas
{
    [SerializeField] private CameraMainMenu _camera;
    [SerializeField] private float _timeForCharConsole;

    [SerializeField] private TextAsset _initConsole;
    [SerializeField] private TextAsset _mainMenuConsoleCommands;

    [SerializeField] private TMP_Text _mainMenuConsole;
    [SerializeField] private TMP_InputField _inputCodeIF;

    [SerializeField] private GameObject _titleButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _firstMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _multiMenu;
    [SerializeField] private GameObject _createLobbyMenu;
    [SerializeField] private GameObject _joinLobbyMenu;
    private Dictionary<MainMenuActionButtons, string> MainMenuCommands { get; set; }

    private void Awake()
    {
        MainMenuCommands = new();
        var listActions = Enum.GetNames(typeof(MainMenuActionButtons));
        var commands = _mainMenuConsoleCommands.text.Split("___", StringSplitOptions.RemoveEmptyEntries);
        MainMenuActionButtons action = MainMenuActionButtons.None;
        for (int i = 0; i < commands.Length; i++)
        {
            var command = commands[i].Split(":\t", StringSplitOptions.RemoveEmptyEntries)[0].Split("_");
            for (int j = 0; j < listActions.Length; j++)
            {
                if (listActions[j] == command[0])
                {
                    action = (MainMenuActionButtons)Enum.GetValues(typeof(MainMenuActionButtons)).GetValue(j);
                    MainMenuCommands.Add(action, command[1]);
                    break;
                }
            }
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

    public void RestartConsoles()
    {
        _mainMenuConsole.text = "";
        _camera.RestartCamera();
        StartCoroutine(WriteConsole(_initConsole.text, ShowTitle));
    }

    public void OnTitleButtonClick()
    {
        _titleButton.SetActive(false);
        StartCoroutine(StartMenu());
    }

    private void ShowTitle()
    {
        _restartButton.SetActive(false);
        _titleButton.SetActive(true);
        _firstMenu.SetActive(false);
        _gameMenu.SetActive(false);
        _multiMenu.SetActive(false);
        _createLobbyMenu.SetActive(false);
        _joinLobbyMenu.SetActive(false);
    }

    private void ShowFirstMenu()
    {
        _firstMenu.SetActive(true);
    }

    public void OnCreditsButtonClick()
    {
        _firstMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Credits], ShowFirstMenu));
    }

    public void OnGameButtonClick()
    {
        _firstMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Game], ShowGameModes));
    }

    private void ShowGameModes()
    {
        _gameMenu.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        _firstMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Exit], Application.Quit));
    }

    public void OnTutorialButtonClick()
    {
        _gameMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Tutorial], LoadTutorial));
    }

    private void LoadTutorial()
    {
        CanvasScene.Singleton.ChangeScene(Scene.Tutorial);
    }

    public void OnMultiButtonClick()
    {
        _gameMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Multi], ShowMultiModes));
    }

    private void ShowMultiModes()
    {
        _multiMenu.SetActive(true);
    }

    public void OnSingleButtonClick()
    {
        _gameMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Single], LoadSingleGame));
    }

    private void LoadSingleGame()
    {
        CanvasScene.Singleton.ChangeScene(Scene.Game);
    }

    public void OnFastMatchButtonClick()
    {
        _multiMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.FastMatch], LoadFastMatch));
    }

    private void LoadFastMatch()
    {
        CanvasScene.Singleton.ChangeScene(Scene.Game);
    }

    public void OnCreateLobbyClick()
    {
        _multiMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.CreateLobby], LoadAdminGame));
    }

    private void LoadAdminGame()
    {
        CanvasScene.Singleton.ChangeScene(Scene.Game);
    }

    public void OnJoinLobbyButtonClick()
    {
        _multiMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.JoinLobby], ShowJoinLobby));
    }

    private void ShowJoinLobby()
    {
        _joinLobbyMenu.SetActive(true);
    }

    public void OnJoinCodeButtonClick()
    {
        _joinLobbyMenu.SetActive(false);
        StartCoroutine(WriteJoinLobby());
    }

    private void LoadJoinLobby()
    {
        CanvasScene.Singleton.ChangeScene(Scene.Game);
    }

    public void OnConfigButtonClick()
    {
        _firstMenu.SetActive(false);
        StartCoroutine(WriteConsole(MainMenuCommands[MainMenuActionButtons.Config], ShowConfig));
    }

    private void ShowConfig()
    {
        CanvasScene.Singleton.ChangeScene(Scene.Config);
    }

    private IEnumerator StartMenu()
    {
        float time;
        bool animationFinished = false;
        var text = MainMenuCommands[MainMenuActionButtons.Config];
        for (int i = 0; i < text.Length; i++)
        {
            time = _timeForCharConsole;
            if (!animationFinished)
                animationFinished = _camera.DoAnimation(time);
            _mainMenuConsole.text += text[i];
            if (text[i] == Environment.NewLine[0])
                time *= 100;
            yield return new WaitForSecondsRealtime(time);
        }
        ShowFirstMenu();
    }

    private IEnumerator WriteConsole(string text, Action postAction = null)
    {
        float time;
        for (int i = 0; i < text.Length; i++)
        {
            time = _timeForCharConsole;
            _mainMenuConsole.text += text[i];
            if (text[i] == Environment.NewLine[0])
                time *= 100;
            yield return new WaitForSecondsRealtime(time);
        }
        postAction?.Invoke();
    }

    private IEnumerator WriteJoinLobby(Action postAction = null)
    {
        float time;
        var textParts = MainMenuCommands[MainMenuActionButtons.ConnectLobby].Split('?', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < textParts[0].Length; i++)
        {
            time = _timeForCharConsole;
            _mainMenuConsole.text += textParts[0][i];
            if (textParts[0][i] == Environment.NewLine[0])
                time *= 100;
            yield return new WaitForSecondsRealtime(time);
        }
        for (int i = 0; i < _inputCodeIF.text.Length; i++)
        {
            time = _timeForCharConsole;
            _mainMenuConsole.text += _inputCodeIF.text[i];
            if (_inputCodeIF.text[i] == Environment.NewLine[0])
                time *= 100;
            yield return new WaitForSecondsRealtime(time);
        }
        for (int i = 0; i < textParts[1].Length; i++)
        {
            time = _timeForCharConsole;
            _mainMenuConsole.text += textParts[1][i];
            if (textParts[1][i] == Environment.NewLine[0])
                time *= 100;
            yield return new WaitForSecondsRealtime(time);
        }
        LoadJoinLobby();
    }
}
