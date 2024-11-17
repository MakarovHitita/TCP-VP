using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    Disclaimer = 0,
    MainMenu,
    Game,
    Tutorial,
    Config,
}

public interface ISceneCanvas
{
    public void RestartConsoles();
}

public class CanvasScene : MonoBehaviour
{
    public static CanvasScene Singleton { get; private set; }

    private Scene _actualScene;

    [SerializeField] private List<GameObject> _uiSceneParts;
    private Dictionary<Scene, GameObject> SceneUIDic { get; set; }

    private Canvas Canvas { get; set; }

    [SerializeField] private Vector3 _diclaimerScale;
    [SerializeField] private Vector3 _mainMenuScale;
    [SerializeField] private Vector3 _gameScale;
    [SerializeField] private Vector3 _configScale;

    private Vector3 _recTransformPos;
    private RectTransform _rectTransform;

    private void Awake()
    {
        if (Singleton == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            _recTransformPos = _rectTransform.position;
            Singleton = this;

            Canvas = GetComponent<Canvas>();
            _actualScene = 0;
            SceneUIDic = new();
            var list = Enum.GetNames(typeof(Scene)).ToList();
            for (int i = 0; i < _uiSceneParts.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                    if (list[j] == _uiSceneParts[i].name)
                    {
                        SceneUIDic.Add((Scene)Enum.GetValues(typeof(Scene)).GetValue(j), _uiSceneParts[i]);
                        break;
                    }
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

    public void ChangeScene(Scene scene)
    {
        if (scene == Scene.MainMenu)
        {
            Canvas.renderMode = RenderMode.WorldSpace;
            Canvas.sortingLayerID = 1;
            _rectTransform.position = Vector3.zero;
        }
        else if (_actualScene == Scene.MainMenu)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.sortingLayerID = 0;
            _rectTransform.position = _recTransformPos;
        }

        SceneUIDic[_actualScene].SetActive(false);
        SceneUIDic[scene].SetActive(true);
        _actualScene = scene;
        if (scene != Scene.Config)
            SceneUIDic[scene].GetComponent<ISceneCanvas>().RestartConsoles();
        switch (scene)
        {
            case Scene.Disclaimer:
                Canvas.transform.localScale = _diclaimerScale;
                break;
            case Scene.MainMenu:
                Canvas.transform.localScale = _mainMenuScale;
                break;
            case Scene.Game:
            case Scene.Tutorial:
                Canvas.transform.localScale = _gameScale;
                break;
            case Scene.Config:
                Canvas.transform.localScale = _configScale;
                break;
        }
        if (_actualScene != Scene.Config)
            SceneManager.LoadScene((int)scene);
    }
}
