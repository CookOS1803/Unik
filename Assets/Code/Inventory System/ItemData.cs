using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Data", fileName = "New ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private ItemAction _action;

    public new string name => _name;
    public Sprite sprite => _sprite;
    public GameObject prefab => _prefab;
    public ItemAction action => _action;
}
