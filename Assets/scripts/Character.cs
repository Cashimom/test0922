using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [SerializeField] protected float moveSpeed = 5.0f;
    [SerializeField] private float JumpForce = 0.00003f;
    protected Rigidbody rb;
    public Vector3 vector;
    public bool JumpFlg=false;
    public float HP = 100;
    public Weapon Weapon;
    public Transform rightWeaponTransform;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    virtual public void move(Vector3 vector3, float shift)
    {
        transform.Translate(vector3.x * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.y * Time.deltaTime * 5.0f * moveSpeed * shift, vector3.z * Time.deltaTime * 5.0f * moveSpeed * shift);
        var a = transform.eulerAngles;
        a.z = 0.0f;
        vector = a;
        transform.eulerAngles = a;

    }


    public void boostMove(float x, float z)
    {
        Vector3 impulseForce = new Vector3(0, 0, 0);
        if (z > 0)
        {
            impulseForce += transform.forward * JumpForce;
        }
        else if (z < 0)
        {
            impulseForce += transform.forward * -JumpForce;
        }
        if (x > 0)
        {
            impulseForce += transform.right * JumpForce;
        }
        else if (x < 0)
        {
            impulseForce += transform.right * -JumpForce;
        }
        if (x == 0 && z == 0)
        {
            impulseForce += new Vector3(0, JumpForce, 0);
        }
        if (impulseForce != new Vector3(0, 0, 0))
        {
            rb.AddForce(impulseForce, ForceMode.Impulse);
        }
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
            rb.velocity = new Vector3(0, 0, 0);
            JumpFlg = false;
        }));
    }

    public void flyMove(float x, float z)
    {
        var reg = 0.01f;
        Vector3 flyForce = new Vector3(0, 0, 0);
        if (z > 0)
        {
            flyForce += transform.forward * (JumpForce * reg);
        }
        else if (z < 0)
        {
            flyForce += transform.forward * (-JumpForce * reg);
        }
        if (x > 0)
        {
            flyForce += transform.right * (JumpForce * reg);
        }
        else if (x < 0)
        {
            flyForce += transform.right * (-JumpForce * reg);
        }
        if (x == 0 && z == 0)
        {
            flyForce += new Vector3(0, (JumpForce * reg), 0);
        }
        if (flyForce != new Vector3(0, 0, 0))
        {
            rb.AddForce(flyForce, ForceMode.Impulse);
        }
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            //rb.AddForce(new Vector3(0, -0.00003f, 0), ForceMode.Impulse);
            rb.velocity = new Vector3(0, 0, 0);
        }));
    }

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    public virtual bool explodeDamage(float damage)
    {
        HP -= damage;
        return true;
    }

    public virtual bool die()
    {
        Destroy(gameObject);
        return true;
    }
}
