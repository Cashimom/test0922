using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRocket : Weapon
{
    [SerializeField] private float fireTick = 0.1f;
    [SerializeField] private GameObject raser;
    private float fireTime = 0.0f;

    // Use this for initialization
    void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (isHave)
        {
            if (isPlayer)
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 3;
            else
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 10;
            transform.rotation = character.rightWeaponTransform.rotation;

            if (isPlayer&&Input.GetButton("Fire1"))
            {
                Fire1();
            }
            if (isPlayer && Input.GetButtonDown("Fire2"))
            {
                raser.SetActive(true);
            }
            else if (isPlayer && Input.GetButtonUp("Fire2"))
            {
                raser.SetActive(false);
            }
        }
    }

    public override void Fire1()
    {
        fireTime += Time.deltaTime;
        if (fireTime > fireTick)
        {
            fireTime = 0.0f;
            var rs = Fire();
            //rs.moveSpeed = 10;


        }
    }
}
