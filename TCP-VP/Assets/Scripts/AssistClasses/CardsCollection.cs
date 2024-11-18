using System.Collections.Generic;
using UnityEngine;

public class CardsCollection : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listCards;
    public List<GameObject> ListCards { get => _listCards; }

    public static CardsCollection Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Singleton = null;
            Destroy(this);
        }
    }
}
