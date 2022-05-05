using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ItemData item;
    private Canvas canvas;
    private Transform player;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image image;
    private Vector2 initialPosition;
    public Transform parent { get; private set; }
    public int index { get; set; }
    public Inventory inventory { get; set; }

    [Inject]
    void SetPlayer(PlayerController controller)
    {
        player = controller.transform;
    }

    void Start()
    {   
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        parent = transform.parent;

        initialPosition = rectTransform.anchoredPosition;
    }

    public void SetItem(ItemData data)
    {
        item = data;
        image.sprite = data.sprite;
        image.color = new Color(1f, 1f, 1f, 1f);
    }

    public void UnsetItem()
    {
        item = null;
        image.sprite = null;
        image.color = new Color(1f, 1f, 1f, 0f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(parent.parent);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parent);
        
        if (UserRaycaster.IsBlockedByUI())
        {            
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = initialPosition;
            return;
        }
        
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

        Instantiate(item.prefab, spawnPosition, Quaternion.identity);
        inventory[index] = null;

    }
}
