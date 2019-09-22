using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject setting;

    [SerializeField] Slider mouseSensitivity;

    [SerializeField] Slider fieldOfView;

    [SerializeField] GameObject _inventory;

    [SerializeField] Inventory inventory;


    [NonSerialized] public bool isOpen = true;

    private PlayerController player;

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
        this.player = player;
        gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isOpen = true;
        InventoryOpen();
    }

    public Dictionary<string, float> Exit()
    {
        gameObject.SetActive(false);
        Dictionary<string, float> settings = new Dictionary<string, float>();
        //if (player != null) player.RotationSensitivity = mouseSensitivity.value;
        //if (camera != null) camera.fieldOfView = fieldOfView.value;
        settings["mouseSensitivity"] = mouseSensitivity.value;
        settings["fieldOfView"] = fieldOfView.value;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isOpen = false; 
        return settings;
    }

    public void InventoryOpen()
    {
        setting.SetActive(false);
        //_inventory.SetActive(true);

        //inventory.gameObject.SetActive(true);
        inventory.Open(player);
    }

    public void SettingOpne()
    {
        setting.SetActive(true);
        inventory.Exit();
    }
}
