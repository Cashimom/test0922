using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : Weapon
{

    [SerializeField] public int Ammunition = 20;

    [SerializeField] public float maxAngle = 15;

    // Start is called before the first frame update
    new void Start()
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
                Vector3 pos = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 2;
                pos -= character.rightWeaponTransform.up * 3;
                pos -= character.rightWeaponTransform.forward * (-2);
                transform.position = pos;
            }
            else
                transform.position = character.transform.position + character.transform.forward * 20;
            transform.rotation = character.rightWeaponTransform.rotation;
            transform.Rotate(0, 180, 0);

            if (isHave)
            {
                if (fireTime < fireTick)
                    fireTime += Time.deltaTime;
                if (isPlayer && Input.GetButton("Fire1"))
                {
                    Fire1();
                }
            }
        }
    }

    public override void Fire1()
    {
        float _maxAngle = maxAngle;
        if (isPlayer&&Input.GetKey(KeyCode.Mouse1))
        {
            _maxAngle *= 0.5f;
        }
        if (fireTime >= fireTick)
        {
            fireTime = 0;
            for(int i = 0; i < Ammunition; i++)
            {
                var f = Fire();
                var rx = Random.value*(2) -1;
                var ry = Random.value*(2) -1;
                var anglex = (Random.value * (2) - 1) * _maxAngle;
                var angley = (Random.value * (2) - 1) * _maxAngle;
                f.transform.localPosition += new Vector3(rx*2, ry*2, 0.1f*i);

                f.transform.rotation *= Quaternion.Euler(anglex, angley , 0);
            }
            var anime = gameObject.GetComponent<Animator>();
            anime.Play("Reload");
        }
    }
}
