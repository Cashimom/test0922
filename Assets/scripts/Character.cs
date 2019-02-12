using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// キャラクターを作るときに継承させるクラス。
/// extended in <seealso cref="playerController"/>
/// <seealso cref="EnemyController"/>
/// </summary>
public class Character : MonoBehaviour
{
    /// <summary>
    /// <see cref="move(Vector3, float)"/>するときのスピード
    /// </summary>
    [SerializeField] protected float moveSpeed = 5.0f;

    /// <summary>
    /// <see cref="flyMove(float, float)"/>と
    /// <see cref="boostMove(float, float)"/>
    /// をするときの強さ
    /// </summary>
    [SerializeField] private float FlyForce = 200;

    /// <summary>
    /// キャラクターのHP
    /// </summary>
    [SerializeField] public float HP = 100;

    /// <summary>
    /// 持っている武器
    /// </summary>
    [SerializeField] public Weapon Weapon;

    /// <summary>
    /// キャラクターのtransform.eulerAnglesを保存している変数。
    /// used in <see cref="FollowPlayer"/>
    /// </summary>
    [NonSerialized] public Vector3 vector;

    /// <summary>
    /// <see cref="Weapon"/>を置く場所、
    /// </summary>
    [NonSerialized] public Transform rightWeaponTransform;

    /// <summary>
    /// Rigidbodyを持っておく変数
    /// </summary>
    protected Rigidbody rb;

    /// <summary>
    /// ブースト中かどうか
    /// </summary>
    protected bool boostFlg = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 基本移動
    /// </summary>
    /// <param name="vector3">移動する方向</param>
    /// <param name="shift">スピードの倍率。used when Dashing</param>
    virtual public void move(Vector3 vector3, float shift)
    {
        transform.Translate(vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift);
        var a = transform.eulerAngles;
        a.z = 0.0f;
        vector = a;
        transform.eulerAngles = a;

    }

    /// <summary>
    /// ブースト
    /// </summary>
    /// <param name="x">方向x</param>
    /// <param name="z">方向z</param>
    public void boostMove(float x, float z)
    {
        Vector3 impulseForce = new Vector3(0, 0, 0);
        if (z > 0)
        {
            impulseForce += transform.forward * FlyForce;
        }
        else if (z < 0)
        {
            impulseForce += transform.forward * -FlyForce;
        }
        if (x > 0)
        {
            impulseForce += transform.right * FlyForce;
        }
        else if (x < 0)
        {
            impulseForce += transform.right * -FlyForce;
        }

        if (x == 0 && z == 0)
        {
            impulseForce += new Vector3(0, FlyForce, 0);
        }

        if (impulseForce != new Vector3(0, 0, 0))
        {
            rb.AddForce(impulseForce, ForceMode.Impulse);
            boostFlg = true;
            boostFlg = true;
            StartCoroutine(DelayMethod2(0.1f, () =>
            {
                boostFlg = false;
                //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(0, 0, 0);
                /*var vel = rb.velocity;
                vel.x = vel.z = 0;
                rb.velocity = vel;*/
            }));
        }
    }

    /// <summary>
    /// 滞空しながら移動
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void flyMove(float x, float z)
    {
        var reg = 0.01f;
        Vector3 flyForce = new Vector3(0, 0, 0);
        if (z > 0)
        {
            flyForce += transform.forward * (FlyForce * reg);
        }
        else if (z < 0)
        {
            flyForce += transform.forward * (-FlyForce * reg);
        }
        if (x > 0)
        {
            flyForce += transform.right * (FlyForce * reg);
        }
        else if (x < 0)
        {
            flyForce += transform.right * (-FlyForce * reg);
        }
        if (x == 0 && z == 0)
        {
            flyForce += new Vector3(0, (FlyForce * reg), 0);
        }
        if (flyForce != new Vector3(0, 0, 0)&&!boostFlg)
        {
            rb.AddForce(flyForce, ForceMode.Force);

        }
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            
            if (!boostFlg)
                //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(0, 0, 0);
                /*var vel = rb.velocity;
                vel.x = vel.z = 0;
                rb.velocity = vel;*/
        }));

    }

    /// <summary>
    /// 時間差で処理させれるやつ
    /// </summary>
    /// <param name="waitTime">待つ時間</param>
    /// <param name="action">実行する処理</param>
    /// <returns>知らん</returns>
    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    /// <summary>
    /// 時間差で処理させれるやつ2
    /// </summary>
    /// <param name="waitTime">待つ時間</param>
    /// <param name="action">実行する処理</param>
    /// <returns>知らん</returns>
    public IEnumerator DelayMethod2(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    /// <summary>
    /// 爆発によるダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns></returns>
    public virtual bool explodeDamage(float damage)
    {
        HP -= damage;
        return true;
    }

    /// <summary>
    /// 死ぬ処理
    /// </summary>
    /// <returns></returns>
    public virtual bool die()
    {
        if (Weapon != null)
        {
            Weapon.DropWeapon();
        }
        Destroy(gameObject);
        return true;
    }

    /// <summary>
    /// デバッグ用のテキストを表示する
    /// </summary>
    /// <param name="str"></param>
    protected void debugText(string str)
    {
        var tmp = GameObject.Find("Canvas/kasokudo").GetComponent<TextMeshProUGUI>();
        tmp.text = str;
    }
}
