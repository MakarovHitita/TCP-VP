using TMPro;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour, ISceneCanvas
{
    [SerializeField] private TextAsset _mainMenuConsoleCommands;

    [SerializeField] private TMP_Text _mainMenuConsole;

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
        throw new System.NotImplementedException();
    }
}
