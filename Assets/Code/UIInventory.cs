using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    
    void Start()
    {
        
    }

    public void SetInventory(Inventory newInventory)
    {
        inventory = newInventory;
        inventory.Add(new Item(ItemAssets.assets[0]));
        inventory[4] = new Item(ItemAssets.assets[1]);
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        for (int i = 0; i < inventory.size; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            
            if (inventory[i] == null)
            {
                image.sprite = null;
                image.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                image.sprite = inventory[i].data.sprite;
                image.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
