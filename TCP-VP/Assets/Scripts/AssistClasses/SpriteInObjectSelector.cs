using TMPro;
using UnityEngine;

public class SpriteInObjectSelector : MonoBehaviour
{
    [SerializeField] private GameObject _objectReference;
    private string Name { get; set; }
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Name = _objectReference.name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ConfigBehaviour.Singleton.AddOnNotifyDaltonismChangesEvent(ChangeDaltonismSprite);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeDaltonismSprite(DaltonismType type)
    {
        _spriteRenderer.sprite = SpriteManager.Singleton.SpriteDictionary[type][Name];
    }

    private void OnDestroy()
    {
        ConfigBehaviour.Singleton.RemoveOnNotifyDaltonismChangesEvent(ChangeDaltonismSprite);
    }
}
