using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour, ISceneCanvas, ISceneUI
{
    // Declaración de títulos y textos
    [SerializeField] private List<GameObject> _listTitles;
    [SerializeField] private List<GameObject> _listTexts;
    private List<GameObject> _listUIs;

    //Estado
    private int _state;

    private void Awake()
    {
        _state = 0;

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        if (arg1 == SceneManager.GetSceneByBuildIndex((int)Scene.Tutorial))
        {
            _listUIs ??= CardsCollection.Singleton.ListCards;

            _listTitles[_state].SetActive(true);
            _listTexts[_state].SetActive(true);
            _listUIs[_state].SetActive(true);
        }
    }

    public void OnButtonPressed()
    {
        _state++;

        _listTitles[_state - 1].SetActive(false);
        _listTexts[_state - 1].SetActive(false);
        _listUIs[_state - 1].SetActive(false);
        if (_state == _listTitles.Count)
        {
            CanvasScene.Singleton.ChangeScene(Scene.MainMenu);
            return;
        }
        _listTitles[_state].SetActive(true);
        _listTexts[_state].SetActive(true);
        _listUIs[_state].SetActive(true);
    }

    public void RestartConsoles()
    {
        Awake();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
