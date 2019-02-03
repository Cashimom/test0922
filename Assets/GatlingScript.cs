using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingScript : Weapon {

    private float fireTime = 0.0f;
    public float fireTick = 0.1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (isHave)
        {
            transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 2;
            transform.rotation= character.rightWeaponTransform.rotation;
            transform.Rotate(0, -90, 0);

            if (Input.GetButton("Fire1"))
            {
                fireTime += Time.deltaTime;
                if (fireTime > fireTick)
                {
                    fireTime = 0.0f;
                    var rs = Fire();
                    rs.moveSpeed = 5;


                }
            }
        }
	}
}
