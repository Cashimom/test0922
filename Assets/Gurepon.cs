using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gurepon : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (character != null)
        {
            if (isPlayer)
            {
                Vector3 pos = character.rightWeaponTransform.position + character.rightWeaponTransform.right * (-6);
                pos -= character.rightWeaponTransform.up * 3;
                pos -= character.rightWeaponTransform.forward * (4);
                transform.position = pos;
            }
            else
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * WeaponTransformDistance;
            transform.rotation = character.rightWeaponTransform.rotation;
            //transform.Rotate(0, 180, 0);

            if (fireTime < fireTick)
                fireTime += Time.deltaTime;
            if (isPlayer)
            {
                GameObject.Find("Canvas").GetComponent<UIController>().setGrenade(fireTime / fireTick);
            }
            if (isHave)
            {
                if (isPlayer && Input.GetButton("Fire1"))
                {
                    Fire1();
                }
            }
        }
    }

    public override void Fire1()
    {
        if (fireTime >= fireTick)
        {
            fireTime = 0;
            var f= Fire();
            f.GetComponent<Rigidbody>().AddForce(f.transform.forward*2000+ f.transform.up*400, ForceMode.Impulse);
            //var anime = gameObject.GetComponent<Animator>();
            //anime.Play("Reload");
        }
    }
}
