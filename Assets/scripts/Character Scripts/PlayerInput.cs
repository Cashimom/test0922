using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Serialization;

using CodeHelper;

/// <summary>
/// キーボードの入力を受け取ってプレイヤーの動作のフラグを立てる
/// </summary>
public class PlayerInput:MonoBehaviour
{
    public float mouseSensitivity = 500f;

    public float moveForward
    {
        get { return Mathf.Clamp01(Input.GetAxis("Vertical"))* (Input.GetButton("Vertical") ? 1 : 0); }
    }
    public float moveBack
    {
        get { return Mathf.Clamp01(-Input.GetAxis("Vertical"))* (Input.GetButton("Vertical") ? 1 : 0); }
    }
    public float moveRight 
    { 
        get { return Mathf.Clamp01(Input.GetAxis("Horizontal"))* (Input.GetButton("Horizontal") ? 1 : 0); }
    }
    public float moveLeft
    {
        get { return Mathf.Clamp01(-Input.GetAxis("Horizontal"))* (Input.GetButton("Horizontal") ? 1 : 0); }
    }

    private bool Jump=false;
    public bool jump
    {
        get { return (Jump=Input.GetButtonDown("Jump")); }
    }

    private bool Rise = false;
    public bool rize
    {
        get { return(Rise= Input.GetKey(KeyCode.LeftShift)); }
    }

    public int boostForward
    {
        get { return (Rise && Jump && moveForward > 0)?1:0; }
    }
    public int boostRight
    {
        get { return (Rise && Jump && moveRight > 0)?1:0; }
    }
    public int boostLeft
    {
        get { return (Rise && Jump && moveLeft > 0)?1:0; }
    }
    public int boostBack
    {
        get { return (Rise && Jump && moveBack > 0)?1:0; }
    }

    public float rotationHorizontal
    {
        get { return Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity; }
    }
    public float rotationVertical
    {
        get { return -Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity; }
    }

    public bool pickUp
    {
        get { return Input.GetKeyDown(KeyCode.E); }
    }
    public int weaponChange
    {
        get
        {
            return (int)Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));
        }
    }
    public bool weaponSwitch
    {
        get { return Input.GetKeyDown(KeyCode.Q); }
    }
    public int weaponDrop = -1;

    public bool shield
    {
        get { return Input.GetKeyDown("c"); }
    }
    public bool fire1
    {
        get { return Input.GetButton("Fire1"); }
    }
    public bool fire2
    {
        get { return Input.GetButton("Fire2"); }
    }



    void Update()
    {
    }
}