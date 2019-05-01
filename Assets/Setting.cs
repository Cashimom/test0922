using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 設定を反映させたりする
/// </summary>
public class Setting : MonoBehaviour
{

    [SerializeField] GameObject menu;

    [SerializeField] Slider mouseSensitivity;

    [SerializeField] Slider fieldOfView;

    [SerializeField] playerController player;

    [SerializeField] Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeSelf) MenuExit();
            else MenuOpen();
        }
    }

    void MenuOpen()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void MenuExit()
    {
        menu.SetActive(false);
        if(player!=null)player.RotationSensitivity = mouseSensitivity.value;
        if(camera!=null)camera.fieldOfView = fieldOfView.value;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
