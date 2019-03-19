using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャノンのスクリプト
/// /// extends <see cref="Weapon"/>
/// </summary>
public class ShotRocket : Weapon
{
    /// <summary>
    /// Fire2を押しているときに出すレーザーポインター
    /// </summary>
    [SerializeField] private GameObject raser;

    // Use this for initialization
    void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (character!=null)
        {
            if (isPlayer)
            {
                Vector3 pos= character.rightWeaponTransform.position + character.rightWeaponTransform.right * 3;
                pos -= character.rightWeaponTransform.up*3;
                pos -= character.rightWeaponTransform.forward * 2;
                transform.position = pos;
            }
            else
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * WeaponTransformDistance;
            transform.rotation = character.rightWeaponTransform.rotation;

            if (isHave)
            {
                if (fireTime < fireTick)
                    fireTime += Time.deltaTime;
                if (isPlayer && Input.GetButton("Fire1"))
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
    }

    /// <summary>
    /// Fire1が押されているときの処理。
    /// see <see cref="Weapon.Fire1"/>
    /// </summary>
    public override void Fire1()
    {
        if (fireTime >= fireTick)
        {
            fireTime = 0.0f;
            var rs = Fire();
            //rs.moveSpeed = 10;
            var anime = gameObject.GetComponent<Animator>();
            anime.Play("Reload");

        }
    }
}
