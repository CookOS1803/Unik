using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public Inventory inventory { get; private set; }
    [SerializeField] private GameObject itemPrefab;
    
    void Start()
    {
        
    }

    public void SetInventory(Inventory newInventory)
    {
        inventory = newInventory;
        RefreshInventory();
        inventory.onChange += (object sender, System.EventArgs e) => RefreshInventory();

        for (int i = 0; i < inventory.size; i++)
        {
            transform.GetChild(i).GetComponent<ItemSlot>().index = i;
        }
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < inventory.size; i++)
        {
            ItemSlot slot = transform.GetChild(i).GetComponent<ItemSlot>();
            
            if (inventory[i] == null)
            {
                slot.DestroyItem();
            }
            else
            {
                slot.SetItem(itemPrefab, inventory[i]);
            }
        }
    }
}
