using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInventory : MonoBehaviour
{
    public Inventory inventory { get; private set; }
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Color defaultSlotColor;
    [SerializeField] private Color selectedSlotColor;
    private RawImage selectedSlotImage;

    [Inject]
    void SetInventory(PlayerController player)
    {
        inventory = player.inventory;

        RefreshInventory();
        inventory.onChange += RefreshInventory;
        inventory.onSlotSelection += SelectSlot;

        for (int i = 0; i < inventory.size; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<ItemSlot>().index = i;
            child.GetComponent<RawImage>().color = defaultSlotColor;
        }

        selectedSlotImage = transform.GetChild(inventory.selectedSlot).GetComponent<RawImage>();
        selectedSlotImage.color = selectedSlotColor;
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
                slot.SetItem(itemPrefab, inventory[i].data);
            }
        }
    }

    public void SelectSlot()
    {
        selectedSlotImage.color = defaultSlotColor;
        
        selectedSlotImage = transform.GetChild(inventory.selectedSlot).GetComponent<RawImage>();
        selectedSlotImage.color = selectedSlotColor;
    }
}
