using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, ISceneCanvas, ISceneUI
{   
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _defeatScreen;

    void Awake()
    {        
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        if (arg1 == SceneManager.GetSceneByBuildIndex((int)Scene.Game))
        {
            _gameScreen.SetActive(true);
            _victoryScreen.SetActive(false);
            _defeatScreen.SetActive(false);
        }
    }

    public void OnButtonPressedVictory()
    {
        _gameScreen.SetActive(false);
        _victoryScreen.SetActive(true);
    }

   public  void OnButtonPressedDefeat()
    {
        _gameScreen.SetActive(false);
        _defeatScreen.SetActive(true);
    }

    public void RestartConsoles()
    {
        Awake();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void OnButtonScreen()
    {
        CanvasScene.Singleton.ChangeScene(Scene.MainMenu);
        return;
    }
}