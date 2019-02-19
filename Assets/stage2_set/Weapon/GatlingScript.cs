using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガトリングのスクリプト。
/// extends <see cref="Weapon"/>
/// </summary>
public class GatlingScript : Weapon
{
    /// <summary>
    /// 弾を撃つ間隔
    /// </summary>
    [SerializeField] private float fireTick = 0.1f;

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
        if (isHave&&character!=null)
        {
            if(isPlayer)
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * 2;
            else 
                transform.position = character.rightWeaponTransform.position + character.rightWeaponTransform.right * WeaponTransformDistance;

            transform.rotation= character.rightWeaponTransform.rotation;
            transform.Rotate(0, -90, 0);

            if (isPlayer && Input.GetButton("Fire1"))
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


        }
    }
}
