using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : Weapon
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHave && character != null)
        {
            if (isPlayer)
            {
                Vector3 pos = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 2;
                pos -= character.rightWeaponTransform.up * 3;
                pos -= character.rightWeaponTransform.forward * (-2);
                transform.position = pos;
            }
            else
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * WeaponTransformDistance;
            transform.rotation = character.rightWeaponTransform.rotation;
            transform.Rotate(0, 180, 0);

            if (fireTime < fireTick)
                fireTime += Time.deltaTime;
            if (isPlayer && Input.GetButton("Fire1"))
            {
                Fire1();
            }
        }
    }

    public override void Fire1()
    {
        if (fireTime >= fireTick)
        {
            fireTime = 0;
            for(int i = 0; i < 20; i++)
            {
                var f = Fire();
                var rx = Random.value*(2) -1;
                var ry = Random.value*(2) -1;
                f.transform.localPosition += new Vector3(rx*2, ry*2, 0.1f*i);
                f.transform.rotation *= Quaternion.Euler(rx * 15, ry * 15, 0);
            }
            var anime = gameObject.GetComponent<Animator>();
            anime.Play("Reload");
        }
    }
}
