using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [SerializeField] private float deathTime = 1.0f;

    [NonSerialized] public bool killedByNotPlayer = false;

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

    private Material deathMaterial;

    private float sceneStartTime = 0.0f;

    // Use this for initialization
    void Start ()
    {
        HP = MaxHP;
        rb = GetComponent<Rigidbody>();
        if(rightWeaponTransform==null)rightWeaponTransform = transform;
        deathMaterial= Resources.Load<Material>("Jouhatu");
        sceneStartTime = Time.time;
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
            if (RightWeapon != null)
            {
                var len_xz = target.transform.position - RightWeapon.ShotTransform.position;
                //len_xz.y = 0;
                //debugText(len_xz.magnitude.ToString());
                if (len_xz.magnitude > (RightWeapon.ShotTransform.position - transform.position).magnitude*1.5f)
                {
                    transform.rotation = Quaternion.LookRotation(target.transform.position - RightWeapon.ShotTransform.position);
                }
                else
                {

                }


                //targetの方向に動く
                if (isEDF)
                {
                    //オブジェクトの固有ナンバーからどっちに動くか判断
                    UnityEngine.Random.InitState(gameObject.GetHashCode());
                    var len = target.transform.position - RightWeapon.ShotTransform.position;
                    if (len.magnitude > 75)
                    {
                        move(new Vector3(UnityEngine.Random.value*8-4, 0, (float)Math.Sqrt(len.magnitude) / 10), 1.6f);
                    }
                    else
                    {
                        move(new Vector3((UnityEngine.Random.value*5-2.5f)
                            , UnityEngine.Random.Range(-1, 1) * 0.2f, 0), 1.6f);
                    }
                }

            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            }

        }



        if (RightWeapon != null&&target!=null)
        {

            delayCnt++;
            if (delayCnt >= delay)
            {
                delayCnt = 0;
                
                //Raycastを使って向いてる方向に障害物がないかチェックして撃つ
                int layerMask = 3 << 9;
                RaycastHit hit;
                if (Physics.Raycast(RightWeapon.ShotTransform.position, RightWeapon.ShotTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)&&((hit.point-target.transform.position).magnitude <= 10||(hit.collider.gameObject!=null&&hit.collider.gameObject.layer==9)))
                {
                    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    RightWeapon.Fire1();

                }
                else
                {
                    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                }
                
            }
        }

	}

    /// <summary>
    /// 爆発によるダメージの処理をする。
    /// see <see cref="Character.explodeDamage(float)"/>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns>死んだかどうか</returns>
    public override bool explodeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            if (isEDF && RightWeapon != null) 
            {
                RightWeapon.character = null;
                Destroy(RightWeapon.gameObject);
            }
            die();
            return true;
        }
        return false;
    }

    public override bool die()
    {
        deathMaterial.SetFloat("_Period", deathTime);
        deathMaterial.SetFloat("_StartTime", Time.fixedTime );
        var ren = transform.GetComponentsInChildren<Renderer>();
        foreach (var r in ren)
        {
            r.material = deathMaterial;
        }
        //GetComponent<Renderer>().material = deathMaterial;
        if (RightWeapon != null)
        {
            RightWeapon.DropWeapon();
        }
        if (LeftWeapon != null)
        {
            LeftWeapon.DropWeapon();
        }
        if (rb != null)
        {
            rb.Sleep();
        }
        if (isEDF)
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
        Destroy(gameObject,deathTime);
        this.enabled = false;
        return true;//base.die();
    }

    /// <summary>
    /// 爆発によるダメージの処理をする。
    /// see <see cref="Character.explodeDamage(float)"/>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns>死んだかどうか</returns>
    public override bool explodeDamage(float damage,Character whose)
    {
        HP -= damage;
        if (HP <= 0)
        {
            if (isEDF && RightWeapon != null)
            {
                RightWeapon.character = null;
                Destroy(RightWeapon.gameObject);
            }
            if (!(whose is PlayerController)) {
                gameObject.SetActive(false);
                if (RightWeapon != null)
                    RightWeapon.gameObject.SetActive(false);
                killedByNotPlayer = true;
            }
            else
            {
                die();
            }
            return true;
        }
        return false;
    }

}
