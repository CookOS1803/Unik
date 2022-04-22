using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Data", fileName = "New ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _prefab;

    public new string name
    {
        get => _name;
        private set => _name = value;
    }

    public Sprite sprite
    {
        get => _sprite;
        private set => _sprite = value;
    }

    public GameObject prefab
    {
        get => _prefab;
        private set => _prefab = value;
    }
}
