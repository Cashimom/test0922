using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの処理を実装しているクラス
/// extends <see cref="Character"/>
/// </summary>
public class PlayerController : Character
{
    /// <summary>
    /// 胴体のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject body;

    /// <summary>
    /// 頭のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject head;

    /// <summary>
    /// "Press F"って表示させるためのテキスト
    /// </summary>
    [SerializeField] private GameObject pressButton;

    [SerializeField] private List<Weapon> WeaponList;



    //public PlayerContainer playerContainer;

    /// <summary>
    /// マウスの感度
    /// </summary>
    [SerializeField] public float RotationSensitivity = 1000f;// 感度

    /// <summary>
    /// <see cref="Jump"/>する強さ
    /// </summary>
    [SerializeField] private float jumpForce = 50;

    public float OnFloorHeight = 1.2f;

    private Weapon nearWeapon;
    /// <summary>
    /// 拾うことができる近くのアイテム。
    /// setterで<see cref="pressButton"/>を切り替え。
    /// set in <see cref="Weapon.OnTriggerEnter(Collider)"/>
    ///  and <see cref="Weapon.OnTriggerExit(Collider)"/>
    /// </summary>
    public Weapon NearWeapon {
        set
        {
            this.nearWeapon = value;
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
            
        }
        get
        {
            return this.nearWeapon;
        }
    }

    //E押したらポーズ
    public bool pause = false;

    [SerializeField] public PlayerJob playerJob;

    

    public List<PlayerJob> jobs;



    private GameObject shield;
    
    /// <summary>
    /// 2段ジャンプのフラグ
    /// </summary>
    private bool secondJumpFlg = false;

    /// <summary>
    /// <see cref="Update"/>でJumpが押されていたか保持する変数
    /// </summary>
    private bool isJumpPressed = false;

    /// <summary>
    /// <see cref="Update"/>でQが押されていたか保持する変数
    /// </summary>
    private bool isKeyQPressed = false;

    /// <summary>
    /// <see cref="Update"/>でJumpが押されていたか保持する変数
    /// </summary>
    private bool isKeyFPressed = false;

    /// <summary>
    /// エネルギーのチャージ時間カウント用変数
    /// </summary>
    private float chargeTimeCnt = 0;

    private UIController uiController;

    /// <summary>
    /// HPがなくなって死ぬときの処理を入れておく。
    /// insert by <see cref="GameSystem"/>
    /// </summary>
    public event Action dieFunc;

    private Character myShield;


    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rightWeaponTransform = head.transform;
        shield = (GameObject)Resources.Load("Shield");

        if (pressButton == null)
        {
            pressButton = GameObject.Find("Canvas/PickUp View");
        }
        pressButton.SetActive(false);
        var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        tmp.text = HP.ToString();
        uiController = GameObject.Find("Canvas").GetComponent<UIController>();

        if (LeftWeapon != null){
            uiController.SlotUpdate(LeftWeapon, 2);
            LeftWeapon.isHave = false;
        }

        if (playerJob != null)
        {
            playerJob.player = this;
            playerJob._Start();
        }
    }

    public virtual void Update()
    {

        //int onObject = 3 << 9;
        RaycastHit objectHit;
        bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out objectHit, OnFloorHeight);
        if (isHit)
        {
            chargeTimeCnt += Time.deltaTime;

            if (chargeTimeCnt > 0.1)
            {
                if(Energy<MaxEnergy)
                    Energy += 1;
                chargeTimeCnt = 0;
            }

            if (secondJumpFlg)
                secondJumpFlg = false;
        }

        ChangeEnergyText();


        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isKeyQPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isKeyFPressed = true;
        }


        //gravity,速度制限付き！
        if(rb.velocity.y > -8)rb.AddForce(-rb.transform.up * 50, ForceMode.Acceleration);


        if (playerJob != null)
        {
            playerJob._Update();
        }
    }

    void FixedUpdate()
    {

        //マウスで方向を変える
        var rotX = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        var rotY = -Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
        Rotation(rotX, rotY);

        //Eが押されたらpause状態をswitch
        if (Input.GetKeyDown(KeyCode.E))
        {
            pause = !pause;
        }
        //pause状態ならマウスカーソルをだす
        CursorLock(!pause);
        
        //武器拾う
        if (isKeyFPressed&&NearWeapon!=null)
        {
            PickUpWeapon(NearWeapon);
            NearWeapon = null;
        }

        //武器入れ替え
        if (NowWeapon == WEAPON_RIGHT)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeWeapon(1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeWeapon(-1);
            }
        }


        if (isKeyQPressed)
        {
            switchWeapon();
        }

        //wasdとかで動かす
        float shiftValue = 1.0f;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //shiftValue = 2.0f;
        }
        var mx = Input.GetAxis("Horizontal");
        var mz = Input.GetAxis("Vertical");
        move(new Vector3(mx,0,mz), shiftValue);


        if (Input.GetButtonDown("Jump"))
        {
            //isJumpPressed = true;
        }

        //ジャンプする
        if (isJumpPressed)
        {
            Jump();
        }

        //ブーストする
        if(isJumpPressed&&Input.GetKey(KeyCode.LeftShift)&& Energy>=5)
        {
            Energy -= 5;
            boostMove(mx, mz);
        }
        if (boostFlg)
        {
            boostDecay();
        }

        //滞空する
        if (Input.GetKey(KeyCode.LeftShift) && !boostFlg&& Energy>=5*Time.deltaTime)
        {
            Energy -= (5 * Time.deltaTime);
            flyMove(mx, mz);
        }

        //降下する
        if (Input.GetKeyDown("z")&& Energy>=5)
        {
            Energy -= (5);
            rb.AddForce(new Vector3(0, -50f, 0), ForceMode.Impulse);
        }
        if (Input.GetKey("z") && Energy >= 5*Time.deltaTime)
        {
            Energy -= (5 * Time.deltaTime);
            rb.AddForce(new Vector3(0, -100f, 0), ForceMode.Force);
        }

        //バリア張る
        if (Input.GetKeyDown("c"))
        {
            if (myShield == null && Energy >= 10) {
                Energy -= 10;
                var shieldObj = Instantiate(shield, head.transform.position + head.transform.forward * 10, transform.rotation * head.transform.localRotation);
                //shieldObj.transform.rotation
                myShield = shieldObj.GetComponent<Character>();
            }
            else if(myShield != null)
            {
                myShield.die();
                myShield = null;
            }
        }
        if (myShield != null)
        {
            myShield.transform.position = head.transform.position + head.transform.forward * 10;
            myShield.transform.rotation = transform.rotation * head.transform.localRotation;
        }


        isJumpPressed = false;
        isKeyQPressed = false;
        isKeyFPressed = false;
    }

    /// <summary>
    /// 基本移動。RigidBodyを使った移動にしている。
    /// see <see cref="Character.move(Vector3, float)"/>
    /// </summary>
    /// <param name="vector3">移動する方向</param>
    /// <param name="shift">スピードの倍率</param>
    public override void move(Vector3 vector3, float shift)
    {
        //transform.Translate(vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift);
        //body.transform.Translate(vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift);
        //rb.AddForce(vector3 * shift*0.00001f,ForceMode.Force);
        float mx = (vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Cos(transform.rotation.eulerAngles.y/180*Mathf.PI) + (vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Sin(transform.rotation.eulerAngles.y / 180 * Mathf.PI);
        float mz= (vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Cos(transform.rotation.eulerAngles.y / 180 * Mathf.PI) - (vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Sin(transform.rotation.eulerAngles.y / 180 * Mathf.PI);
        rb.MovePosition(rb.position + (new Vector3( mx, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, mz)));
        //transform.localPosition= new Vector3(0, 1.7f, 0);
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out objectHit, 1.2f))
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
    /// カーソルをロックする
    /// </summary>
    /// <param name="pause">ロックするか否か</param>
    private void CursorLock(bool pause)
    {
        if (pause)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// xの入力とyの入力によって回転させる
    /// </summary>
    /// <param name="rotX">Y軸の回転 ( input X ) </param>
    /// <param name="rotY">X軸の回転 ( input Y )</param>
    public void Rotation(float rotX,float rotY)
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
    public void PickUpWeapon(Weapon picked)
    {
        //枠がいっぱいなら手持ちと交換
        if (WeaponList.Count >= 2)
        {
            int index = WeaponList.FindIndex(match => RightWeapon==match);
            WeaponList[index] = picked;
            RightWeapon.DropWeapon();
            picked.HaveWeapon();
            RightWeapon = picked;
        }
        //何も持ってなかったらすぐ装備
        else if (RightWeapon == null&& WeaponList.Count<2)
        {
            WeaponList.Add(picked);
            picked.HaveWeapon();
            RightWeapon = picked;
        }
        //何か持ってて枠が空いてたら装備せずに拾う
        else if (RightWeapon != null && WeaponList.Count < 2)
        {
            WeaponList.Add(picked);
            picked.PickWeapon();
        }
        uiController.SlotUpdate(WeaponList);
        uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
    }

    /// <summary>
    /// 武器を持ち替える
    /// </summary>
    /// <param name="sign">持ち替える方向(正負)</param>
    public void ChangeWeapon(int sign)
    {
        if (NowWeapon == WEAPON_LEFT)
        {
            switchWeapon();
        }
        var nowIndex = WeaponList.FindIndex(match => match == RightWeapon);
        if (WeaponList.Count >= 2)
        {
            Weapon will;
            if (sign>0&&nowIndex + 1 >= WeaponList.Count)
            {
                will = WeaponList[0];
            }
            else if(sign>0&&nowIndex+1<WeaponList.Count)
            {
                will = WeaponList[nowIndex+1];
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

        uiController.SlotUpdate(WeaponList);
        uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
    }

    void switchWeapon()
    {
        if (NowWeapon == WEAPON_RIGHT&&LeftWeapon!=null)
        {
            NowWeapon = WEAPON_LEFT;
            if(RightWeapon!=null)
                RightWeapon.isHave = false;
            LeftWeapon.isHave = true;
            uiController.SetActiveSlot(2);
        }
        else if (NowWeapon == WEAPON_LEFT&&RightWeapon!=null)
        {
            NowWeapon = WEAPON_RIGHT;
            RightWeapon.isHave = true;
            if(LeftWeapon!=null)
                LeftWeapon.isHave = false;
            uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
        }
    }

    public override bool explodeDamage(float damage)
    {
        base.explodeDamage(damage);
        var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        tmp.text = ((int)HP).ToString();
        //var slider = GameObject.Find("Canvas/HP Slider").GetComponent<Slider>();
        //slider.value = (HP / MaxHP);
        uiController.Damage(HP, MaxHP);
        if (HP<=0&&dieFunc!=null)
            dieFunc();
        return true;
    }

    public override bool explodeDamage(float damage,Character character)
    {
        base.explodeDamage(damage,character);
        var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        tmp.text = ((int)HP).ToString();
        //var slider = GameObject.Find("Canvas/HP Slider").GetComponent<Slider>();
        //slider.value = (HP / MaxHP);
        uiController.Damage(HP, MaxHP);
        if (HP <= 0 && dieFunc != null)
            dieFunc();
        return true;
    }
}
