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

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;

            Canvas = GetComponent<Canvas>();
            _actualScene = 0;
            SceneUIDic = new();
            var list = Enum.GetNames(typeof(Scene)).ToList();
            for (int i = 0; i < _uiSceneParts.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                    if (list[j].Split('_', StringSplitOptions.RemoveEmptyEntries)[0] == _uiSceneParts[i].name)
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
        }
        else if (_actualScene == Scene.MainMenu)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.sortingLayerID = 0;
        }

        SceneUIDic[_actualScene].SetActive(false);
        SceneUIDic[scene].SetActive(true);
        _actualScene = scene;
        if (scene != Scene.Config)
            SceneUIDic[scene].GetComponent<ISceneCanvas>().RestartConsoles();
        if (_actualScene != Scene.Config)
            SceneManager.LoadScene((int)scene);
    }
}