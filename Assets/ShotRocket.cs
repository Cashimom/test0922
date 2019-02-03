using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRocket : Weapon
{
    private float fireTime = 0.0f;
    public float fireTick = 0.1f;
    public GameObject raser;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (isHave)
        {
            transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 3;
            transform.rotation = character.rightWeaponTransform.rotation;

            if (Input.GetButton("Fire1"))
            {
                fireTime += Time.deltaTime;
                if (fireTime > fireTick)
                {
                    fireTime = 0.0f;
                    var rs = Fire();
                    rs.moveSpeed = 10;
                    
                    
                }
            }
            if (Input.GetButtonDown("Fire2"))
            {
                raser.SetActive(true);
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                raser.SetActive(false);
            }
        }
    }
}
