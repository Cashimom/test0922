using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガトリングのスクリプト。
/// extends <see cref="Weapon"/>
/// </summary>
public class GatlingScript : Weapon
{

    AudioSource audio;
    // Use this for initialization
    void Start () {
        base.Start();

        audio=GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (character!=null)
        {
            if (isPlayer)
            {
                Vector3 pos = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 2;
                pos -= character.rightWeaponTransform.up * 2;
                pos -= character.rightWeaponTransform.forward * (-2);
                transform.position = pos;
            }
            else
                //transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * WeaponTransformDistance;
                transform.position = character.transform.position+character.transform.forward*20;

            transform.rotation= character.rightWeaponTransform.rotation;
            transform.Rotate(0, -90, 0);

            if (isHave&&isPlayer && Input.GetButton("Fire1"))
            {
                Fire1();
            }
        }
	}

    /// <summary>
    /// Fire1が押されているときの処理。
    /// see <see cref="Weapon.Fire1"/>
    /// </summary>
    public override void Fire1()
    {
        fireTime += Time.deltaTime;
        if (fireTime > fireTick)
        {
            fireTime = 0.0f;
            var rs = Fire();
            //rs.moveSpeed = 5;
            
            //audio.Play();

        }
    }

    public override void setWeaponInfo()
    {
        base.setWeaponInfo();
    }
}
