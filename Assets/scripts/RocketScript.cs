using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Weaponから発射するロケット用のクラス
/// </summary>
public class RocketScript : MonoBehaviour
{

    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField] public float moveSpeed = 1.0f;

    /// <summary>
    /// ヒットした時に出すパーティクル
    /// </summary>
    [SerializeField] protected ParticleSystem destroyParticle;

    /// <summary>
    /// characterにヒットした時のダメージ
    /// </summary>
    [SerializeField] public float explodeDamageValue = 10.0f;

    /// <summary>
    /// 弾が存在できる時間(射程)
    /// </summary>
    [SerializeField] public float TimeLimit = 10;

    /// <summary>
    /// ヒットしたあと弾が残る時間
    /// </summary>
    [SerializeField] public float explodeDelay = 5 / 6;

    [NonSerialized] public Character parent;

    [NonSerialized] public Vector3 v0 = new Vector3(0,0,0);
    
    /// <summary>
    /// Rigidbodyが保存されてる変数
    /// </summary>
    protected Rigidbody rb;

    /// <summary>
    /// ヒットしたかどうか
    /// </summary>
    private bool isCollisionEntered = false;

    /// <summary>
    /// <see cref="destroyParticle"/>の動作時間
    /// </summary>
    protected float destroyTime = 0;

    /// <summary>
    /// 時間カウント用変数
    /// </summary>
    protected float t=0;

    /// <summary>
    /// サイズ変更時の元のサイズ
    /// </summary>
    protected Vector3 defaultScale;

    /// <summary>
    /// 生成されてからの時間
    /// </summary>
    protected float flyTime = 0;
    

	// Use this for initialization
	void Start ()
    {
        //transform.Rotate(rotX, rotY, rotZ);
        rb = GetComponent<Rigidbody>();
        if(destroyParticle!=null)
            destroyTime = destroyParticle.main.duration;
        rb.AddForce(transform.forward * 50 * moveSpeed, ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (!isCollisionEntered)
        {
            //transform.Translate(0, 0, 1 * moveSpeed);
            rb.AddForce(transform.forward*10*moveSpeed+v0);
        }
        else
        {
            t += Time.deltaTime;
            float sc = (1 - t / destroyTime);
            //transform.localScale = defaultScale * (sc);
        }

        flyTime += Time.deltaTime;
        if (flyTime >= TimeLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollisionEntered)
        {
            Hit();
            var otherObj = collision.gameObject;
            if (otherObj.GetComponent<Character>() !=null)
            {
                otherObj.GetComponent<Character>().explodeDamage(explodeDamageValue,parent);
            }
        }
    }

    /// <summary>
    /// ヒットした時の処理
    /// </summary>
    public void Hit()
    {
        GetComponent<MeshRenderer>().enabled = false;
        isCollisionEntered = true;
        var particle = Instantiate(destroyParticle, transform);
        particle.Play();
        Destroy(gameObject, destroyTime+(explodeDelay));
        rb.Sleep();
        defaultScale = transform.localScale;
        var collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size *= 5;
        var sound=GetComponent<AudioSource>();
        if (sound != null)
            sound.Play();
        
    }

    private void OnTriggerEnter(Collider other)
    {

        var otherObj = other.gameObject;
        if (otherObj.GetComponent<Character>() != null)
        {
            otherObj.GetComponent<Character>().explodeDamage(explodeDamageValue*0.8f,parent);
        }

        if (isCollisionEntered&&other.gameObject.tag=="Rocket")
        {
            StartCoroutine(DelayMethod((explodeDelay), () =>
            {
                if (other != null) { }
                    //other.gameObject.GetComponent<RocketScript>().Hit();
            }));
        }

    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
