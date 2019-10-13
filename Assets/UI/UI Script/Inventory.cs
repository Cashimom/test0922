using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] WeaponSlot slot1;

    [SerializeField] WeaponSlot slot2;

    [SerializeField] WeaponInfoPanel infoPanel1;

    [SerializeField] WeaponInfoPanel infoPanel2;

    private PlayerController player;

    [NonSerialized] public WeaponSlot dragging;

    [SerializeField] DropPanel dropPanel;

    private void Awake()
    {
        slot1.inventory = slot2.inventory = dropPanel.inventory = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open(PlayerController player)
    {
        gameObject.SetActive(true);
        this.player = player;
        SlotUpdate();

    }

    void SlotUpdate()
    {

        if (player.WeaponList.Count >= 1)
        {
            infoPanel1.weapon = slot1.weapon = player.WeaponList[0];
            slot1.gameObject.SetActive(true);
        }
        else
        {
            infoPanel1.weapon = slot1.weapon= null;
            slot1.gameObject.gameObject.SetActive(false);
        }
        if (player.WeaponList.Count >= 2)
        {
            infoPanel2.weapon = slot2.weapon = player.WeaponList[1];
            slot2.gameObject.SetActive(true);
        }
        else
        {
            infoPanel2.weapon = slot2.weapon = null;
            slot2.gameObject.SetActive(false);
        }

        player.UpdateWeaponUI();
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        dragging = null;
    }

    public void WeaponDrop(Weapon weapon)
    {
        player.DropWeapon(weapon);
        SlotUpdate();
    }

    public void WeaponExchange(Weapon weapon1,Weapon weapon2)
    {
        var i1=player.WeaponList.FindIndex(match => match == weapon1);
        var i2=player.WeaponList.FindIndex(match => match == weapon2);
        player.WeaponList[i1] = weapon2;
        player.WeaponList[i2] = weapon1;
        SlotUpdate();
    }
}
