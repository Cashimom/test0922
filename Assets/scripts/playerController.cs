using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerController : Character {
    
    public float RotationSensitivity = 1000f;// 感度
    public float rotX=0.0f, rotY=0.0f;
    //private bool JumpFlg = false;
    //public Vector3 vector;
    //public GameObject rocket;
    public GameObject body;
    public GameObject head;
    private bool secondJumpFlg = false;
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
    }

    void FixedUpdate()
    {

        //マウスで方向を変える
        rotX += Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        rotY += -Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
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
        transform.Rotate(0, rotX, 0);

        //wasdとかで動かす
        float shiftValue = 1.0f;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftValue = 2.0f;
        }
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        move(new Vector3(x,0,z), shiftValue);


        //var tmp = GameObject.Find("Canvas/kasokudo").GetComponent<TextMeshProUGUI>();
        //tmp.text = rb.velocity.y.ToString();
        //ジャンプする
        if (Input.GetButtonDown("Jump"))
        {
            if ((Math.Abs(rb.velocity.y) < 0.0005))
            {
                rb.AddForce(0, 50, 0,ForceMode.Impulse);
                secondJumpFlg = false;
            }
            else if (!secondJumpFlg)
            {
                rb.velocity = new Vector3(0, 0, 0);
                rb.AddForce(0, 50, 0, ForceMode.Impulse);
                secondJumpFlg = true;
            }
            
        }
        if(Input.GetButtonDown("Jump")&&Input.GetKey(KeyCode.LeftShift))
        {
            JumpFlg = true;
            boostMove(x, z);

        }
        if (/*Input.GetButton("Jump")*/Input.GetKey(KeyCode.LeftShift) && !JumpFlg)
        {
            flyMove(x, z);

        }
        if (/*Input.GetButtonUp("Jump")*/Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(DelayMethod(0.1f, () =>
            {
                //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(0, 0, 0);
            }));

        }
        if (Input.GetKeyDown("z"))
        {
            rb.AddForce(new Vector3(0, -100f, 0), ForceMode.Impulse);
            StartCoroutine(DelayMethod(0.1f, () =>
            {
                //rb.AddForce(new Vector3(0, 0.00003f, 0), ForceMode.Impulse);
                rb.velocity = new Vector3(0, 0, 0);
            }));
        }
        //if (Input.GetButton("Fire1"))
        //{
        //    fireTime += Time.deltaTime;
        //    if (fireTime > fireTick)
        //    {
        //        fireTime = 0.0f;
        //        var fire = Instantiate(rocket, transform.position + transform.forward * 5, transform.rotation);
        //    }
        //}
        //rightWeaponTransform = head.transform;
    }

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

    

}
