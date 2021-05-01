using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class DropPanel : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized] public Inventory inventory;

    private Image image;

    private Color originalColor;

    public void OnDrop(PointerEventData eventData)
    {
        inventory.dragging.ReturnToFirstPos();
        inventory.WeaponDrop(inventory.dragging.weapon);
        image.color = originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj != null&&obj.GetComponent<WeaponSlot>()!=null)
        {
            originalColor = image.color;
            image.color = Color.Lerp(image.color, Color.red, 0.8f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj != null && obj.GetComponent<WeaponSlot>() != null)
        {
            image.color = originalColor;
        }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        transform.SetSiblingIndex(2);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
