using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    [SerializeField] private ItemData data;

    public Item GetItem()
    {
        return new Item(data);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
