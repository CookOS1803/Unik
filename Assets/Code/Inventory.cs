using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IEnumerable
{
    private Item[] items;
    public System.EventHandler onChange;
    
    public int size => items.Length;
    public Item this[int i]
    {
        get => items[i];
        set
        {
            items[i] = value;

            if (onChange != null)
                onChange(this, new System.EventArgs());
        }
    }

    public Inventory()
    {
        items = new Item[10];
    }

    public void Add(Item newItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                if (onChange != null)
                    onChange(this, new System.EventArgs());
                return;
            }
        }

        Debug.LogError("Inventory is full");
    }

    public void SwapSlots(int first, int second)
    {
        if (first == second)
            return;

        if (!HasIndex(first) || !HasIndex(second))
        {
            Debug.LogError("Index is out of range");
            return;
        }

        Item tempItem = items[first];
        items[first] = items[second];
        items[second] = tempItem;

        if (onChange != null)
            onChange(this, new System.EventArgs());
    }
    
    public IEnumerator GetEnumerator()
    {
        foreach (Item i in items)
        {
            yield return i;
        }
    }

    public bool HasIndex(int i)
    {
        return i >= 0 && i < items.Length;
    }
}
