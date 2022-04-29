using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IEnumerable
{
    private Item[] items;
    private int _selectedSlot = 0;
    public event EventHandler onChange;
    public event EventHandler onSlotSelection;
    
    public Transform owner { get; set; }
    public int size => items.Length;
    public int selectedSlot
    {
        get => _selectedSlot;
        set
        {
            _selectedSlot = value;

            onSlotSelection?.Invoke(this, EventArgs.Empty);
        }
    }
    public Item this[int i]
    {
        get => items[i];
        set
        {
            items[i] = value;

            onChange?.Invoke(this, EventArgs.Empty);
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
                onChange?.Invoke(this, EventArgs.Empty);

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

        onChange?.Invoke(this, EventArgs.Empty);
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

        onChange?.Invoke(this, EventArgs.Empty);
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
