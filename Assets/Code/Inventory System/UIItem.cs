using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Item _item;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image image;
    private Vector2 initialPosition;
    public int index { get; set; }
    public Inventory inventory { get; set; }

    public Item item
    {
        get => _item;
        set
        {
            _item = value;
            image.sprite = _item.data.sprite;
        }
    }

    void Awake()
    {   
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        initialPosition = rectTransform.anchoredPosition;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UserRaycaster.IsBlockedByUI())
        {            
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = initialPosition;
            return;
        }

        var player = GameObject.FindWithTag("Player").transform;

        RaycastHit hit;
        Vector3 spawnPosition;

        if (Physics.Raycast(player.position + player.up, player.forward + player.up, out hit, 1f))
        {
            spawnPosition = hit.point;
        }
        else
        {
            spawnPosition = player.position + player.forward + player.up;
        }

        Instantiate(item.data.prefab, spawnPosition, Quaternion.identity);
        inventory[index] = null;

    }
}
