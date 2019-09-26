using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class WeaponSlot : MonoBehaviour , IBeginDragHandler,IDragHandler,IEndDragHandler, IDropHandler
{

    [SerializeField] public RawImage image;

    [NonSerialized] public Inventory inventory;

    private Weapon _weapon;
    [SerializeField] public Weapon weapon
    {
        get { return _weapon; }
        set
        {
            _weapon=value;
            image.texture = _weapon.image;
        }
    }

    private Vector2 firstPosition;


    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.position= eventData.position;
        transform.SetSiblingIndex(0);
        inventory.dragging = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(1);
        inventory.dragging = null;
        ReturnToFirstPos();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventory.dragging != this)
        {
            inventory.WeaponExchange(weapon, inventory.dragging.weapon);
            //var w = inventory.dragging.weapon;
            //inventory.dragging.weapon = weapon;
            //weapon = w;
        }
    }



    private void Awake()
    {
        image = gameObject.GetComponent<RawImage>();

        transform.SetSiblingIndex(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
        //rect = gameObject.GetComponent<Rect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToFirstPos()
    {
        transform.position = firstPosition;
    }

}
