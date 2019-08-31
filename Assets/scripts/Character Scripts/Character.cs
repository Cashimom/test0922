using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// キャラクターを作るときに継承させるクラス。
/// extended in <seealso cref="PlayerController"/>
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
    [SerializeField] private float FlyForce = 100;

    //[FormerlySerializedAs("HP")]
    private float hp;
    /// <summary>
    /// キャラクターのHP
    /// </summary>
    /// 
    
    [SerializeField] public float HP {
        set {
            value = Mathf.Clamp(value, 0, MaxHP);
            float delta = hp - value;
            hp = value;
            ChangeHPText(value,delta);
        }
        get { return hp; }
    }
    
    /// <summary>
    /// キャラクターのHPの上限
    /// </summary>
    [SerializeField] public float MaxHP = 100;

    /// <summary>
    /// キャラクターのエネルギーの上限
    /// </summary>
    [SerializeField] public float MaxEnergy = 100;

    /// <summary>
    /// 持っている武器
    /// </summary>
    [SerializeField] public Weapon RightWeapon;
    
    /// <summary>
    /// 持っている武器2
    /// </summary>
    [SerializeField] public Weapon LeftWeapon;

    /// <summary>
    /// キャラクターのtransform.eulerAnglesを保存している変数。
    /// used in <see cref="FollowPlayer"/>
    /// </summary>
    [NonSerialized] public Vector3 vector;

    /// <summary>
    /// <see cref="RightWeapon"/>を置く場所、
    /// </summary>
    [SerializeField] public Transform rightWeaponTransform;


    private float energy=100;
    /// <summary>
    /// キャラクターの保持エネルギー
    /// </summary> 
    public float Energy {
        set { this.energy = Math.Min(value , MaxEnergy); ChangeEnergyText(); }
        get { return this.energy; }
    }

    /// <summary>
    /// Rigidbodyを持っておく変数
    /// </summary>
    protected Rigidbody rb;

    /// <summary>
    /// ブースト中かどうか
    /// </summary>
    protected bool boostFlg = false;

    protected float boostTimeCnt = 0;

    public const int WEAPON_RIGHT = 1;
    public const int WEAPON_LEFT = 2;
    public int NowWeapon = WEAPON_RIGHT;


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
        if (z != 0)
        {
            impulseForce += transform.forward * FlyForce * (z > 0 ? 1 : -1);
        }
        if (x != 0)
        {
            impulseForce += transform.right * FlyForce * (x > 0 ? 1 : -1);
        }

        if (x == 0 && z == 0)
        {
            impulseForce += transform.up * FlyForce*2;
        }

        if (impulseForce != new Vector3(0, 0, 0))
        {
            rb.AddForce(impulseForce, ForceMode.Impulse);
            boostFlg = true;
            boostTimeCnt = 0;
        }
    }

    /// <summary>
    /// ブーストした後のスピードの減衰
    /// </summary>
    public void boostDuring()
    {
        if (boostFlg)
        {
            boostTimeCnt += Time.deltaTime;
            if (boostTimeCnt >= 1.0f)
            {
                boostFlg = false;
                boostTimeCnt = 0;
                return;
            }

            var vel = rb.velocity;
            rb.AddForce(-(vel * 2f), ForceMode.Force);
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

            //ブーストによる上方向への加速度,速度制限付き！
            //debugText(rb.velocity.y.ToString());
            if(rb.velocity.y < 64) {
                rb.AddForce(rb.transform.up * 200, ForceMode.Acceleration);
            }
            else if (rb.velocity.y > 96)
            { 
                {
                    var vel = rb.velocity;
                    vel.y /= 10;
                    rb.velocity = vel;
                }
            }

        }
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            
            if (!boostFlg)
                //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
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
    /// 爆発によるダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns></returns>
    public virtual bool explodeDamage(float damage,Character whose)
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
        if (RightWeapon != null)
        {
            RightWeapon.DropWeapon();
        }
        if (LeftWeapon != null)
        {
            LeftWeapon.DropWeapon();
        }
        Destroy(gameObject);
        return true;
    }

    /// <summary>
    /// エネルギー量を表示しているテキストを更新する
    /// </summary>
    protected virtual void ChangeEnergyText()
    {
        //var tmp = GameObject.Find("Canvas/ShowEnergy Text").GetComponent<TextMeshProUGUI>();
        //tmp.text = ((int)energy).ToString();
        //var slider = GameObject.Find("Canvas/Energy Slider").GetComponent<Slider>();
        //slider.value = (energy / MaxEnergy);
    }

    protected virtual void ChangeHPText(float hp,float delta)
    {

    }

    /// <summary>
    /// デバッグ用のテキストを表示する
    /// </summary>
    /// <param name="str"></param>
    public void debugText(string str)
    {
        var tmp = GameObject.Find("Canvas/kasokudo").GetComponent<TextMeshProUGUI>();
        tmp.text = str;
    }
}
