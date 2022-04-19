using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private ItemData _data;
    public ItemData data => _data;

    public Item(ItemData newData)
    {
        _data = newData;
    }
}
