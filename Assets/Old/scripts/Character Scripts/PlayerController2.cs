using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Serialization;
using CodeHelper;
using Players;

public class PlayerController2 : Character
{
    [SerializeField] private PlayerModel playerModel;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] public PlayerWeapon playerWeapon;

    /// <summary>
    /// "Press F"って表示させるためのテキスト
    /// </summary>
    [SerializeField] private GameObject pressButton;

    private UIController uiController;


    [SerializeField] public PlayerJob playerJob;

    public List<PlayerJob> jobs;

    private int jobIndex;

    /// RightWeaponを置く場所
    [SerializeField] public Transform rightWeaponTransform;

    public Weapon NearWeapon;


    private void Awake()
    {

        if (pressButton == null)
        {
            pressButton = GameObject.Find("Canvas/PickUp View");
        }
        uiController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    // Use this for initialization
    void Start()
    {
        CursorLock(true);
        pressButton.SetActive(false);

        if (playerModel.LeftWeapon != null)
        {
            uiController.SlotUpdate(playerModel.LeftWeapon, 2);
        }

        if (jobs.Count > 0)
        {
            jobIndex = -1;
            playerJob = null;
        }
        if (playerJob != null)
        {
            // TODO: 
            //playerJob.player = this;
            //playerJob._Start();
        }
    }

    // Update is called once per frame
    void Update()
    {

        playerModel.Rotation(playerInput.rotationHorizontal, playerInput.rotationVertical);

        var mx = playerInput.moveRight - playerInput.moveLeft;
        var mz = playerInput.moveForward - playerInput.moveBack;
        if (true)
        {
            playerModel.Move(vec.vec3(mx,0,mz), 1);
        }

        if (playerInput.rize)
        {
            playerModel.flyMove(mx, mz);
        }

        if (playerInput.boostRight!=0|| playerInput.boostLeft!=0|| 
            playerInput.boostForward!=0|| playerInput.boostBack!=0)
        {
            playerModel.boostMove(
                playerInput.boostRight - playerInput.boostLeft,
                playerInput.boostForward - playerInput.boostBack);
        }

        if (playerInput.jump)
        {
            playerModel.Jump();
        }


        if (playerInput.pickUp)
        {
            playerWeapon.WeaponPickUp(playerModel.NearWeapon);
        }

        if (playerInput.weaponChange!=0)
        {
            playerWeapon.WeaponChange(playerInput.weaponChange);
        }

        if (playerInput.weaponSwitch)
        {
            playerWeapon.WeaponSwitch();
        }

        if (playerInput.shield)
        {
            playerModel.Sheld();
        }
    }

    private void FixedUpdate()
    {

        playerModel.EnergyChargeOnFloor();

    }


    /// <summary>
    /// カーソルをロックする
    /// </summary>
    /// <param name="pause">ロックするか否か</param>
    private void CursorLock(bool pause)
    {
        if (pause)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}