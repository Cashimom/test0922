using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// エネミーの処理を実装しているクラス
/// extends <see cref="Character"/>
/// </summary>
public class EnemyController : Character
{
    /// <summary>
    /// 動きのサイクル時間
    /// </summary>
    [SerializeField] private float cycle = 5.0f;

    /// <summary>
    /// 移動する方向
    /// </summary>
    [SerializeField] private Vector3 moveVec=new Vector3(0,0,0);

    /// <summary>
    /// 弾を撃ったりする目標のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject target;

    /// <summary>
    /// 武器を使う時間(delay倍される)
    /// </summary>
    [SerializeField] private int delay = 2;

    /// <summary>
    /// 時間カウント用変数
    /// </summary>
    private float time = 0.0f;

    /// <summary>
    /// 移動している方向が正か負か
    /// </summary>
    private int wayFlg = 0;

    /// <summary>
    /// <see cref="delay"/>のカウント用変数
    /// </summary>
    private int delayCnt = 0;
    //public float HP = 100;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rightWeaponTransform = transform;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= cycle)
        {
            time = 0.0f;
            wayFlg = 1-wayFlg;
        }
        if (wayFlg == 0)
        {
            move(moveVec, 1);
        }
        else if (wayFlg == 1)
        {
            move(-moveVec, 1);
        }

        if (target != null)
        {
            if (Weapon != null)
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - Weapon.ShotTransform.position);

            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            }
        }



        if (Weapon != null)
        {

            delayCnt++;
            if (delayCnt >= delay)
            {
                delayCnt = 0;
                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 3 << 9;

                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                //layerMask = ~layerMask;

                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(Weapon.ShotTransform.position, Weapon.ShotTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)&&((hit.point-target.transform.position).magnitude <= 10||hit.collider.gameObject.layer==9))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    //Debug.Log("Did Hit");
                    Weapon.Fire1();

                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    //Debug.Log("Did not Hit");
                }
                if(hit.collider!=null)
                    debugText((hit.collider.gameObject.layer).ToString());
                
            }
        }

	}

    /// <summary>
    /// 爆発によるダメージの処理をする。
    /// see <see cref="Character.explodeDamage(float)"/>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns></returns>
    public override bool explodeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            die();
        }
        return true;
    }


}
