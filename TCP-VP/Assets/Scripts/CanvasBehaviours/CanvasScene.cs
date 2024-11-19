using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    Disclaimer = 1,
    MainMenu,
    Game,
    Tutorial,
    Config,
}

public interface ISceneCanvas
{
    public void RestartConsoles();
}

public interface ISceneUI
{
    public void SetActive(bool active);
}

public class CanvasScene : MonoBehaviour
{
    public static CanvasScene Singleton { get; private set; }

    private Scene _actualScene;

    [SerializeField] private List<GameObject> _uiSceneParts;
    private Dictionary<Scene, GameObject> SceneUIDic { get; set; }

    private Canvas Canvas { get; set; }

    [SerializeField] private Vector3 _mainMenuScale;

    private Vector3 _recTransformPos;
    private RectTransform _rectTransform;

    private Camera _camera;

    private void Awake()
    {
        if (Singleton == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            _recTransformPos = _rectTransform.position;
            Singleton = this;
            _camera = FindFirstObjectByType<Camera>();

            Canvas = GetComponent<Canvas>();
            _actualScene = (Scene)1;
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
            Canvas.transform.localScale = _mainMenuScale;
        }
        else if (_actualScene == Scene.MainMenu)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.sortingLayerID = 0;
            //_rectTransform.position = _recTransformPos;
        }

        if (scene == Scene.Config)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if (scene == Scene.Game)
        {
            _camera.gameObject.SetActive(false);
        }

        if (_actualScene == Scene.Game)
        {
            _camera.gameObject.SetActive(true);
        }

        if (_actualScene != 0)
            SceneUIDic[_actualScene].GetComponent<ISceneUI>().SetActive(false);

        SceneUIDic[scene].GetComponent<ISceneUI>().SetActive(true);
        if (scene != Scene.Config)
            SceneUIDic[scene].GetComponent<ISceneCanvas>().RestartConsoles();

        if (scene != Scene.Config)
            SceneManager.LoadScene((int)scene);
        _actualScene = scene;
    }
}
