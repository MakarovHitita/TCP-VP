using UnityEngine;

public class InterSceneObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
