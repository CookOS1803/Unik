using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Inject] private UIInventory uiInventory;
    private UIItem child;
    private Vector2 initialPosition;
    public int index { get; set; }
    
    void Start()
    {
        child = GetComponentInChildren<UIItem>();
        child.inventory = uiInventory.inventory;
        child.index = index;
    }

    public void DestroyItem()
    {
        child.UnsetItem();
    }

    public void SetItem(GameObject prefab, ItemData item)
    {
        child.SetItem(item);        
    }

    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag.GetComponent<UIItem>();

        item.transform.SetParent(item.parent);
        uiInventory.inventory.SwapSlots(index, item.index);
    }
}
