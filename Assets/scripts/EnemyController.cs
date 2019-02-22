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
    [SerializeField] public GameObject target;

    /// <summary>
    /// 武器を使う時間(delay倍される)
    /// </summary>
    [SerializeField] private int delay = 2;

    /// <summary>
    /// EDFモードの切り替え
    /// </summary>
    [SerializeField] private bool isEDF = false;

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
            //targetの方向を向く
            if (Weapon != null)
            {
                var len_xz = target.transform.position - Weapon.ShotTransform.position;
                len_xz.y = 0;
                debugText(len_xz.magnitude.ToString() + " " + (Weapon.ShotTransform.position - transform.position).magnitude.ToString());
                if (len_xz.magnitude > (Weapon.ShotTransform.position - transform.position).magnitude*1.5f)
                {
                    transform.rotation = Quaternion.LookRotation(target.transform.position - Weapon.ShotTransform.position);
                }
                else
                {

                }

            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            }

            //targetの方向に動く
            if (isEDF)
            {
                if ((target.transform.position - Weapon.ShotTransform.position).magnitude > 75)
                {
                    move(new Vector3(0, 0, 2), 1.6f);
                }
                else
                {
                    //オブジェクトの固有ナンバーからどっちに動くか判断
                    Random.InitState(gameObject.GetHashCode());
                    move(new Vector3((((Random.Range(0,3) >> 1) & 1) == 1 ? 1 : -1), Random.Range(-1,1)*0.2f, 0), 1.6f);
                }
            }
        }



        if (Weapon != null)
        {

            delayCnt++;
            if (delayCnt >= delay)
            {
                delayCnt = 0;
                
                //Raycastを使って向いてる方向に障害物がないかチェックして撃つ
                int layerMask = 3 << 9;
                RaycastHit hit;
                if (Physics.Raycast(Weapon.ShotTransform.position, Weapon.ShotTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)&&((hit.point-target.transform.position).magnitude <= 10||(hit.collider.gameObject!=null&&hit.collider.gameObject.layer==9)))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    Weapon.Fire1();

                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                }
                
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
