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
    /// 弾を撃つ間隔
    /// </summary>
    [SerializeField] private float fireTick = 0.1f;

    /// <summary>
    /// Fire2を押しているときに出すレーザーポインター
    /// </summary>
    [SerializeField] private GameObject raser;

    /// <summary>
    /// 時間をカウントする変数
    /// </summary>
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
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * WeaponTransformDistance;
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
            //rs.moveSpeed = 10;


        }
    }
}
