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
    Exit,
}

public class MainMenuCanvas : MonoBehaviour, ISceneCanvas
{
    [SerializeField] private CameraMainMenu _camera;
    [SerializeField] private float _timeForCharConsole;

    [SerializeField] private TextAsset _initConsole;
    [SerializeField] private TextAsset _mainMenuConsoleCommands;

    [SerializeField] private TMP_Text _mainMenuConsole;
    private Dictionary<MainMenuActionButtons, Dictionary<bool, string>> MainMenuCommands { get; set; }

    private void Awake()
    {
        MainMenuCommands = new();
        var listActions = Enum.GetNames(typeof(MainMenuActionButtons));
        var commands = _mainMenuConsoleCommands.text.Split("___", StringSplitOptions.RemoveEmptyEntries);
        MainMenuActionButtons action = MainMenuActionButtons.None;
        for (int i = 0; i < commands.Length; i++)
        {
            var command = commands[i].Split(":\t", StringSplitOptions.RemoveEmptyEntries)[0].Split("_")[0];
            for (int j = 0; j < listActions.Length; j++)
            {
                if (listActions[j] == command)
                {
                    action = (MainMenuActionButtons)Enum.GetValues(typeof(MainMenuActionButtons)).GetValue(j);
                    MainMenuCommands.Add(action, new());
                    break;
                }
            }
            var commandTexts = commands[i].Split("------", StringSplitOptions.RemoveEmptyEntries);
            var c = commandTexts[0].Split(":\t", StringSplitOptions.RemoveEmptyEntries)[1];
            var r = commandTexts[1].Split(":\t", StringSplitOptions.RemoveEmptyEntries)[1];
            MainMenuCommands[action].Add(true, c);
            MainMenuCommands[action].Add(false, r);
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
        StartCoroutine(WriteConsole(_initConsole.text, false));
    }

    private IEnumerator WriteConsole(string text, bool command)
    {
        float time;
        for (int i = 0; i < text.Length; i++)
        {
            time = _timeForCharConsole;
            if (command)
                time *= 10;
            _mainMenuConsole.text += text[i];
            if (text[i] == Environment.NewLine[0])
                time *= 100;
            yield return new WaitForSecondsRealtime(time);
        }
    }
}
