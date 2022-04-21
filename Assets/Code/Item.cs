using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData data { get; private set; }

    public Item(ItemData newData)
    {
        data = newData;
    }
}
