using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider mouseSensitivity;

    [SerializeField] Slider fieldOfView;

    [NonSerialized] public bool isOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isOpen = true;
    }

    public Dictionary<string, float> Exit()
    {
        isOpen = false; 
        gameObject.SetActive(false);
        Dictionary<string, float> settings = new Dictionary<string, float>();
        //if (player != null) player.RotationSensitivity = mouseSensitivity.value;
        //if (camera != null) camera.fieldOfView = fieldOfView.value;
        settings["mouseSensitivity"] = mouseSensitivity.value;
        settings["fieldOfView"] = fieldOfView.value;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        return settings;
    }
}
