using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// プレイヤーの処理を実装しているクラス
/// extends <see cref="Character"/>
/// </summary>
public class playerController : Character
{
    /// <summary>
    /// マウスの感度
    /// </summary>
    [SerializeField] private float RotationSensitivity = 1000f;// 感度

    /// <summary>
    /// 胴体のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject body;

    /// <summary>
    /// 頭のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject head;

    /// <summary>
    /// <see cref="Jump"/>する強さ
    /// </summary>
    [SerializeField] private float jumpForce = 50;
    
    /// <summary>
    /// 2段ジャンプのフラグ
    /// </summary>
    private bool secondJumpFlg = false;

    /// <summary>
    /// <see cref="Update"/>でJumpが押されていたか保持する変数
    /// </summary>
    private bool isJumpPressed = false;

    //E押したらポーズ
    public bool pause = false;

    //void Update()
    //{
    //    var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
    //    var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

    //    transform.Rotate(0, x, 0);
    //    transform.Translate(0, 0, z);
    //}

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rightWeaponTransform = head.transform;
    }

    private void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(head.transform.position, head.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }


        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        debugText(secondJumpFlg.ToString());

        //マウスで方向を変える
        var rotX = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        var rotY = -Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
        Rotation(rotX, rotY);

        //Eが押されたらpause状態をswitch
        if (Input.GetKeyDown(KeyCode.E))
        {
            pause = !pause;
        }
        //pause状態ならマウスカーソルをlock
        CursorLock(pause);

        //wasdとかで動かす
        float shiftValue = 1.0f;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftValue = 2.0f;
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
        if(isJumpPressed&&Input.GetKey(KeyCode.LeftShift))
        {
            boostMove(mx, mz);

        }

        //滞空する
        if (Input.GetKey(KeyCode.LeftShift) && !boostFlg)
        {
            flyMove(mx, mz);

        }
        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    StartCoroutine(DelayMethod(0.1f, () =>
        //    {
        //        //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
        //        rb.velocity = new Vector3(0, 0, 0);
        //    }));

        //}

        //降下する
        if (Input.GetKeyDown("z"))
        {
            rb.AddForce(new Vector3(0, -50f, 0), ForceMode.Impulse);
        }
        if (Input.GetKey("z"))
        {
            rb.AddForce(new Vector3(0, -100f, 0), ForceMode.Force);
        }


        isJumpPressed = false;

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
        //debugText(((float)Math.Cos(transform.rotation.eulerAngles.y / 180 * Mathf.PI)).ToString() + "\n" + ((float)Math.Sin(transform.rotation.eulerAngles.y / 180 * Mathf.PI)).ToString());
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

        if ((Math.Abs(rb.velocity.y) < 0.0005))
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

}
