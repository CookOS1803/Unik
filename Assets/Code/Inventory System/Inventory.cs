using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IEnumerable
{
    private Item[] items;
    private int _selectedSlot = 0;
    public System.EventHandler onChange;
    public System.EventHandler onSlotSelection;
    
    public Transform owner { get; set; }
    public int size => items.Length;
    public int selectedSlot
    {
        get => _selectedSlot;
        set
        {
            _selectedSlot = value;

            if (onSlotSelection != null)
                onSlotSelection(this, new System.EventArgs());
        }
    }
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

    public void UseItem()
    {
        if (items[selectedSlot] == null)
            return;
        
        items[selectedSlot].data.action.Use(owner);
        items[selectedSlot] = null;

        if (onChange != null)
            onChange(this, new System.EventArgs());
    }

    public bool IsFull()
    {
        foreach (var i in items)
        {
            if (i == null)
                return false;
        }

        return true;
    }
}
