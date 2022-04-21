using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private UIInventory uiInventory;
    private Vector2 initialPosition;
    public int index { get; set; }

    void Start()
    {
        uiInventory = GetComponentInParent<UIInventory>();
    }
    
    public void DestroyItem()
    {
        if (transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
    }

    public void SetItem(GameObject prefab, Item item)
    {
        UIItem i;

        if (transform.childCount != 0)
            i = transform.GetChild(0).GetComponent<UIItem>();
        else
            i = Instantiate(prefab, transform).GetComponent<UIItem>();

        i.item = item;
        i.index = index;
        i.inventory = uiInventory.inventory;
    }

    public void OnDrop(PointerEventData eventData)
    {
        uiInventory.inventory.SwapSlots(index, eventData.pointerDrag.GetComponent<UIItem>().index);
    }
}
