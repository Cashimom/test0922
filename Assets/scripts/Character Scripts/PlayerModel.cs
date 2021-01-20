using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Serialization;

using CodeHelper;

/// <summary>
/// プレイヤーの処理の関数をまとめる
/// 別の処理を書いたModelと互換性を持たせたい
/// </summary>
public class PlayerModel : MonoBehaviour
{
    // 頭のゲームオブジェクト
    [SerializeField] private GameObject head;

    private Rigidbody rb;

    private Weapon nearWeapon;
    /// <summary>
    /// 拾うことができる近くのアイテム。
    /// setterで<see cref="pressButton"/>を切り替え。
    /// set in <see cref="Weapon.OnTriggerEnter(Collider)"/>
    ///  and <see cref="Weapon.OnTriggerExit(Collider)"/>
    /// </summary>
    public Weapon NearWeapon
    {
        set
        {
            this.nearWeapon = value;
            //TODO:
            /*
            if (value != null)
            {
                
                if (pressButton != null)
                {
                    pressButton.SetActive(true);
                    pressButton.transform.Find("Weapon Image").GetComponent<RawImage>().texture = nearWeapon.image;
                }
            }
            else
            {
                if (pressButton != null)
                {
                    pressButton.SetActive(false);
                }
            }
            */

        }
        get
        {
            return this.nearWeapon;
        }
    }

    //Jumpする強さ
    [SerializeField] private float jumpForce = 50;

    // 床と判定するオブジェクトまでの最大距離(プレイヤーの中心から)
    public float OnFloorHeight = 1.2f;

    // 2段ジャンプのフラグ
    private bool secondJumpFlg = false;

    // キャラクターのHPの上限
    [SerializeField] public float MaxHP = 100;

    private float hp;
    // キャラクターのHP
    [SerializeField]
    public float HP
    {
        set
        {
            value = Mathf.Clamp(value, 0, MaxHP);
            float delta = hp - value;
            hp = value;
            //ChangeHPText(value, delta);
        }
        get { return hp; }
    }

    // キャラクターのエネルギーの上限
    [SerializeField] public float MaxEnergy = 100;

    private float energy = 100;
    // キャラクターの保持エネルギー
    public float Energy
    {
        set { this.energy = Math.Min(value, MaxEnergy); }
        get { return this.energy; }
    }

    // boostとflyをする時の強さ
    [SerializeField] private float FlyForce = 100;

    // ブースト中かどうか
    protected bool boostFlg = false;

    // ブーストが始まってからの経過時間
    protected float boostTimeCnt = 0;

    // 持っている武器
    [SerializeField] public List<Weapon> WeaponList;

    // 持っている武器
    [SerializeField] public Weapon RightWeapon;

    // 持っている武器2
    [SerializeField] public Weapon LeftWeapon;

    public const int WEAPON_RIGHT = 1;
    public const int WEAPON_LEFT = 2;
    public int NowWeapon = WEAPON_RIGHT;

    /// <summary>
    /// キャラクターのtransform.eulerAnglesを保存している変数。
    /// used in <see cref="FollowPlayer"/>
    /// </summary>
    [NonSerialized] public Vector3 vector;

    // シールドのプレハブ
    private GameObject shield;

    // 表示されている
    private Character myShield;


    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HP = MaxHP;

    }

    private void Start()
    {
        if (LeftWeapon != null)
        {
            LeftWeapon.isHave = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 基本移動。RigidBodyを使った移動
    /// see <see cref="Character.move(Vector3, float)"/>
    /// </summary>
    /// <param name="vector3">移動する方向</param>
    /// <param name="shift">スピードの倍率</param>
    public void Move(Vector3 vector3, float shift)
    {
        vector3 = vector3.normalized;

        float maxMagnitude = 40;
        var rbVec2 = new Vector2(rb.velocity.x, rb.velocity.z);

        var rbVel = (transform.rotation * vector3) * shift * maxMagnitude;
        if (rbVec2.magnitude < maxMagnitude * 1.1)
        {
            //rb.velocity += rbVel+vecVel;

            //FPSによって移動速度が変わらないようにする
            var ratio = 1f - Mathf.Pow(1f - 0.2f, 60f * Time.deltaTime);
            rb.velocity = vec.lerp(rb.velocity, vec.vec3(rbVel.x, rb.velocity.y, rbVel.z), ratio);
        }

        
        var a = head.transform.eulerAngles;
        a.z = 0.0f;
        vector = a;
        head.transform.eulerAngles = a;
        var b = transform.eulerAngles;
        b.z = 0.0f;
        transform.eulerAngles = b;
        
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    public void Jump()
    {
        RaycastHit objectHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out objectHit, OnFloorHeight))
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            secondJumpFlg = false;
        }
        else if (!secondJumpFlg)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            secondJumpFlg = true;
        }
    }

    /// <summary>
    /// 滞空しながら移動
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void flyMove(float x, float z)
    {
        Energy -= (5 * Time.deltaTime);

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
        if (flyForce != new Vector3(0, 0, 0) && !boostFlg)
        {
            rb.AddForce(flyForce, ForceMode.Force);

            //ブーストによる上方向への加速度,速度制限付き！
            //debugText(rb.velocity.y.ToString());
            if (rb.velocity.y < 64)
            {
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
    }

    /// <summary>
    /// ブースト
    /// </summary>
    /// <param name="x">方向x</param>
    /// <param name="z">方向z</param>
    public void boostMove(float x, float z)
    {
        Energy -= 5;

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
            impulseForce += transform.up * FlyForce * 2;
        }

        if (impulseForce != new Vector3(0, 0, 0))
        {
            rb.AddForce(impulseForce, ForceMode.Impulse);
            boostFlg = true;
            boostTimeCnt = 0;
        }
    }

    /// <summary>
    /// xの入力とyの入力によって回転させる
    /// </summary>
    /// <param name="rotX">Y軸の回転 ( input X ) </param>
    /// <param name="rotY">X軸の回転 ( input Y )</param>
    public void Rotation(float rotX, float rotY)
    {
        if (head.transform.forward.y > 0.90f && rotY < 0)
        {
            rotY = 0;
        }
        if (head.transform.forward.y < -0.90f && rotY > 0)
        {
            rotY = 0;
        }
        //transform.Rotate(rotY, rotX, 0.0f);
        head.transform.Rotate(rotY, 0.0f, 0.0f);
        if (head.transform.localRotation.eulerAngles.y > 0)
        {
            var a = head.transform.localRotation.eulerAngles;
            a.y = -0.0005f;
            head.transform.localRotation = Quaternion.Euler(a);
        }
        transform.Rotate(0, rotX, 0);
    }

    /// <summary>
    /// <paramref name="picked"/>を拾う
    /// </summary>
    /// <param name="picked">拾うWeapon</param>
    public void WeaponPickUp(Weapon picked)
    {

        //枠がいっぱいなら手持ちと交換
        if (WeaponList.Count >= 2)
        {
            int index = WeaponList.FindIndex(match => RightWeapon == match);
            WeaponList[index] = picked;
            RightWeapon.DropWeapon();
            picked.HaveWeapon();
            RightWeapon = picked;
        }
        //何も持ってなかったらすぐ装備
        else if (RightWeapon == null && WeaponList.Count < 2)
        {
            WeaponList.Add(picked);
            picked.HaveWeapon();
            RightWeapon = picked;
            if (LeftWeapon != null)
                LeftWeapon.isHave = false;
        }
        //何か持ってて枠が空いてたら装備せずに拾う
        else if (RightWeapon != null && WeaponList.Count < 2)
        {
            WeaponList.Add(picked);
            picked.PickWeapon();
        }

        // TODO: uiの処理をまた別に分離する
        // uiController.SlotUpdate(WeaponList);
        // uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
    }

    /// <summary>
    /// 武器を持ち替える
    /// </summary>
    /// <param name="sign">持ち替える方向(正負)</param>
    public void WeaponChange(int sign)
    {
        if (NowWeapon == WEAPON_LEFT)
        {
            WeaponSwitch();
        }
        var nowIndex = WeaponList.FindIndex(match => match == RightWeapon);
        if (WeaponList.Count >= 2)
        {
            Weapon will;
            if (sign > 0 && nowIndex + 1 >= WeaponList.Count)
            {
                will = WeaponList[0];
            }
            else if (sign > 0 && nowIndex + 1 < WeaponList.Count)
            {
                will = WeaponList[nowIndex + 1];
            }
            else if (sign < 0 && nowIndex <= 0)
            {
                will = WeaponList[WeaponList.Count - 1];
            }
            else if (sign < 0 && nowIndex > 0)
            {
                will = WeaponList[nowIndex - 1];
            }
            else
            {
                will = WeaponList[0];
            }
            //debugText(WeaponList[nowIndex].ToString() +":::"+ Time.time.ToString());
            RightWeapon.ChangeWeapon();
            will.HaveWeapon();
            RightWeapon = will;
        }

        // TODO:
        // uiController.SlotUpdate(WeaponList);
        // uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
    }


    public void WeaponSwitch()
    {
        if (NowWeapon == WEAPON_RIGHT && LeftWeapon != null)
        {
            NowWeapon = WEAPON_LEFT;
            if (RightWeapon != null)
                RightWeapon.isHave = false;
            LeftWeapon.isHave = true;
            // TODO:
            //uiController.SetActiveSlot(2);
        }
        else if (NowWeapon == WEAPON_LEFT && RightWeapon != null)
        {
            NowWeapon = WEAPON_RIGHT;
            RightWeapon.isHave = true;
            if (LeftWeapon != null)
                LeftWeapon.isHave = false;
            //TODO:
            //uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
        }
    }

    public void Sheld()
    {
        if (myShield == null && Energy >= 10)
        {
            Energy -= 10;
            var shieldObj = Instantiate(shield, head.transform.position + head.transform.forward * 10, transform.rotation * head.transform.localRotation);
            //shieldObj.transform.rotation
            myShield = shieldObj.GetComponent<Character>();
        }
        else if (myShield != null)
        {
            myShield.die();
            myShield = null;
        }
    }

    public void EnergyChargeOnFloor()
    {
        RaycastHit objectHit;
        bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out objectHit, OnFloorHeight);
        if (isHit)
        {
            //chargeTimeCnt += Time.deltaTime;

            //if (chargeTimeCnt > 0.1)
            //{
            //    if(Energy<MaxEnergy)
            //        Energy += 1;
            //    chargeTimeCnt = 0;
            //}
            if (Energy < MaxEnergy)
                Energy += 10 * Time.deltaTime;

            if (secondJumpFlg)
                secondJumpFlg = false;
        }
        else
        {
            if (Energy < MaxEnergy)
                Energy += 1 * Time.deltaTime;
        }
    }
}