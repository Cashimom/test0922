using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Serialization;
using CodeHelper;

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

    private UIController uiController;

    /// <summary>
    /// マウスの感度
    /// </summary>
     [FormerlySerializedAs("RotationSensitivity")]
    [SerializeField] public float mouseSensitivity = 1000f;// 感度

    /// <summary>
    /// <see cref="Jump"/>する強さ
    /// </summary>
    [SerializeField] private float jumpForce = 50;

    public float OnFloorHeight = 1.2f;

    /// <summary>
    /// 持っている武器
    /// </summary>
    [SerializeField] public List<Weapon> WeaponList;

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

    private int jobIndex;

    [SerializeField] public float deathAltitude = -400;


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
    private bool isSwitchWeaponPressed = false;

    /// <summary>
    /// <see cref="Update"/>でEが押されていたか保持する変数
    /// </summary>
    private bool isPickWeaponPressed = false;

    /// <summary>
    /// エネルギーのチャージ時間カウント用変数
    /// </summary>
    private float chargeTimeCnt = 0;

    /// <summary>
    /// HPがなくなって死ぬときの処理を入れておく。
    /// insert by <see cref="GameSystem"/>
    /// </summary>
    public event Action dieFunc;

    private Character myShield;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rightWeaponTransform = head.transform;
        shield = (GameObject)Resources.Load("Shield");

        if (pressButton == null)
        {
            pressButton = GameObject.Find("Canvas/PickUp View");
        }
        uiController = GameObject.Find("Canvas").GetComponent<UIController>();
        
    }

    public virtual void Start()
    {
        CursorLock(true);
        pressButton.SetActive(false);

        if (LeftWeapon != null){
            uiController.SlotUpdate(LeftWeapon, 2);
            LeftWeapon.isHave = false;
        }
        if (jobs.Count > 0)
        {
            jobIndex = -1;
            playerJob = null;
        }
        if (playerJob != null)
        {
            playerJob.player = this;
            playerJob._Start();
        }


        HP = MaxHP;

    }

    public virtual void Update()
    {

        //int onObject = 3 << 9;
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

        //ChangeEnergyText();


        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isSwitchWeaponPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPickWeaponPressed = true;
        }


        //gravity,速度制限付き！
        if(rb.velocity.y > -8)rb.AddForce(-rb.transform.up * 50, ForceMode.Acceleration);

        if (Input.GetKeyDown(KeyCode.J))
        {
            jobIndex++;
            if (playerJob!= null)
            {
                playerJob._End();
            }
            if(jobIndex >= jobs.Count)
            {
                jobIndex = -1;
                playerJob = null;
            }
            else
            {
                playerJob = jobs[jobIndex];
                playerJob.player = this;
                playerJob._Start();
            }
            
        }

        if (playerJob != null)
        {
            playerJob._Update();
        }
    }

    void FixedUpdate()
    {

        //マウスで方向を変える
        var rotX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        var rotY = -Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        Rotation(rotX, rotY);

        ////Eが押されたらpause状態をswitch
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    pause = !pause;
        //}
        ////pause状態ならマウスカーソルをだす
        //CursorLock(!pause);
        
        //武器拾う
        if (isPickWeaponPressed&&NearWeapon!=null)
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


        if (isSwitchWeaponPressed)
        {
            SwitchWeapon();
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
        if (isJumpPressed&&Input.GetKey(KeyCode.LeftShift)&& Energy>=5)
        {
            Energy -= 5;
            boostMove(mx*(Input.GetButton("Horizontal")?1:0), mz* (Input.GetButton("Vertical") ? 1 : 0));
        }
        boostDuring();

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

        //高度が基準より低ければ死
        if (transform.position.y < deathAltitude)
        {
            dieFunc();
        }


        isJumpPressed = false;
        isSwitchWeaponPressed = false;
        isPickWeaponPressed = false;
    }

    /// <summary>
    /// 基本移動。RigidBodyを使った移動
    /// see <see cref="Character.move(Vector3, float)"/>
    /// </summary>
    /// <param name="vector3">移動する方向</param>
    /// <param name="shift">スピードの倍率</param>
    public override void move(Vector3 vector3, float shift)
    {
        vector3 = vector3.normalized;
        //transform.Translate(vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift);
        //body.transform.Translate(vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift);
        float maxMagnitude = 40;
        var rbVec2 = new Vector2(rb.velocity.x, rb.velocity.z);
        //var vec2 = new Vector2(vector3.x, vector3.z);
        //var pls = (new Vector2(Mathf.Abs(vec2.x), Mathf.Abs(vec2.y)) + vec2) / 2;
        //var mns = (-new Vector2(Mathf.Abs(vec2.x), Mathf.Abs(vec2.y)) + vec2) / 2;
        //var pVel = (1000f - (25 * pls.magnitude + 1f));
        //var mVel = (1000f - (25 * mns.magnitude + 1f));

        //var rbVel = (transform.rotation * vector3) * shift * 25* (90/*95.80835f謎の定数、この値を超えるとshiftブーストが発生する*/ - rbVec2.magnitude) * Time.deltaTime;
        var rbVel = (transform.rotation * vector3) * shift * maxMagnitude * Mathf.Clamp01((float)Math.Log((rbVec2.magnitude + 2),maxMagnitude)) ;
        //var vecVel = (transform.rotation * vector3) * shift * (1000f - 25*vec2.magnitude ) * Time.deltaTime;
        //rbVel += vecVel;
        debugText(rbVec2.magnitude.ToString() + "\n" + (rbVel).ToString());
        //rb.velocity = new Vector3((rbVel + vecVel).x, rb.velocity.y, (rbVel + vecVel).z);
        if (rbVec2.magnitude < maxMagnitude*1.1)
        {
            //rb.velocity += rbVel+vecVel;

            //FPSによって移動速度が変わらないようにする
            var ratio = 1f - Mathf.Pow(1f - 0.2f, 60f * Time.deltaTime);
            rb.velocity = vec.lerp(rb.velocity,new Vector3(rbVel.x,rb.velocity.y ,rbVel.z),ratio);
        }
            //rb.AddForce((transform.rotation* vector3) * shift*(200f),ForceMode.Force);
        //float mx = (vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Cos(transform.rotation.eulerAngles.y/180*Mathf.PI) + (vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Sin(transform.rotation.eulerAngles.y / 180 * Mathf.PI);
        //float mz= (vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Cos(transform.rotation.eulerAngles.y / 180 * Mathf.PI) - (vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift) * (float)Math.Sin(transform.rotation.eulerAngles.y / 180 * Mathf.PI);
        //rb.MovePosition(rb.position + (new Vector3( mx, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, mz)));
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
            if (LeftWeapon != null)
                LeftWeapon.isHave = false;
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
            SwitchWeapon();
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

    void SwitchWeapon()
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

    public void DropWeapon(Weapon weapon)
    {
        int index = WeaponList.FindIndex(match => weapon == match);
        if(NowWeapon==WEAPON_RIGHT&&RightWeapon == weapon)
        {
            if (WeaponList.Count >= 1)
            {
                ChangeWeapon(1);
            }
            else
            {
                SwitchWeapon();
            }
        }
        WeaponList.Remove(weapon);
        weapon.transform.position = transform.position;
        weapon.DropWeapon();
        uiController.SlotUpdate(WeaponList);
        uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));

    }

    public void UpdateWeaponUI()
    {

        uiController.SlotUpdate(WeaponList);
        if (NowWeapon == WEAPON_RIGHT)
        {
            uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
        }
    }

    //public void setPlayerHP(float hp)
    //{
    //    float delta = HP - hp;
    //    setPlayerHPDelta(delta);
    //}

    //public void setPlayerHPDelta(float delta)
    //{
    //    HP += delta;
    //    HP = Mathf.Min(HP, MaxHP);
    //}

    protected override void ChangeEnergyText()
    {
        var tmp = GameObject.Find("Canvas/ShowEnergy Text").GetComponent<TextMeshProUGUI>();
        tmp.text = ((int)Energy).ToString();
        var slider = GameObject.Find("Canvas/Energy Slider").GetComponent<Slider>();
        slider.value = (Energy / MaxEnergy);
    }

    protected override void ChangeHPText(float hp,float delta)
    {
        var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        tmp.text = ((int)hp).ToString();

        //float delta = hp - HP;
        if (delta > 0)
        {
            uiController.Health(HP, MaxHP);
        }
        else
        {
            uiController.Damage(HP, MaxHP);
        }

        if (HP <= 0 && dieFunc != null)
            dieFunc();
    }

    public override bool explodeDamage(float damage)
    {
        base.explodeDamage(damage);
        //var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        //tmp.text = ((int)HP).ToString();
        //var slider = GameObject.Find("Canvas/HP Slider").GetComponent<Slider>();
        //slider.value = (HP / MaxHP);
        //uiController.Damage(HP, MaxHP);
        //if (HP<=0&&dieFunc!=null)
        //    dieFunc();
        return true;
    }

    public override bool explodeDamage(float damage,Character character)
    {
        base.explodeDamage(damage,character);
        //var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        //tmp.text = ((int)HP).ToString();
        //var slider = GameObject.Find("Canvas/HP Slider").GetComponent<Slider>();
        //slider.value = (HP / MaxHP);
        //uiController.Damage(HP, MaxHP);
        //if (HP <= 0 && dieFunc != null)
        //    dieFunc();
        return true;
    }
}
