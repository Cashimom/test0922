using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 設定を反映させたりする
/// </summary>
public class Setting : MonoBehaviour
{
    //[FormerlySerializedAs("menu")]
    [SerializeField] GameObject _menu;

    [SerializeField] Menu menu;

    //[SerializeField] Slider mouseSensitivity;

    //[SerializeField] Slider fieldOfView;

    [SerializeField] PlayerController player;

    [SerializeField] Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        //_menu.SetActive(false);
        menu.Exit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.isOpen) MenuExit();
            else MenuOpen();
        }
    }

    void MenuOpen()
    {
        menu.Open(player);
        //_menu.SetActive(true);
        //Time.timeScale = 0;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    void MenuExit()
    {
        var setting = menu.Exit();
        //_menu.SetActive(false);
        if(player!=null)player.mouseSensitivity = setting["mouseSensitivity"];
        if(camera!=null)camera.fieldOfView = setting["fieldOfView"];
        //Time.timeScale = 1;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
