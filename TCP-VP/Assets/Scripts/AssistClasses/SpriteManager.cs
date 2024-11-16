using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Singleton { get; private set; }

    [SerializeField] private List<Sprite> _sprites;
    public Dictionary<DaltonismType, Dictionary<string, Sprite>> SpriteDictionary { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            SpriteDictionary = new()
            {
                { DaltonismType.NoDaltonism, new() },
                { DaltonismType.Protanopia, new() },
                { DaltonismType.Deuteranopia, new() },
                { DaltonismType.Tritanopia, new() }
            };
            foreach (var s in _sprites)
            {
                var name = s.name;
                switch (name[0])
                {
                    case 'n':
                        SpriteDictionary[DaltonismType.NoDaltonism].Add(name[1..], s); break;
                    case 'p':
                        SpriteDictionary[DaltonismType.Protanopia].Add(name[1..], s); break;
                    case 'd':
                        SpriteDictionary[DaltonismType.Deuteranopia].Add(name[1..], s); break;
                    case 't':
                        SpriteDictionary[DaltonismType.Tritanopia].Add(name[1..], s); break;

                    default:
                        throw new KeyNotFoundException(name[0].ToString());
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
}
