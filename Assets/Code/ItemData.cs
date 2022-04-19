using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Data", fileName = "New ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;

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
}
