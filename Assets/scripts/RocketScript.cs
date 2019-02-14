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
    [SerializeField] private float moveSpeed = 1.0f;

    /// <summary>
    /// ヒットした時に出すパーティクル
    /// </summary>
    [SerializeField] private ParticleSystem destroyParticle;

    /// <summary>
    /// characterにヒットした時のダメージ
    /// </summary>
    [SerializeField] private float explodeDamageValue = 10.0f;

    /// <summary>
    /// ヒットしたあと弾が残る時間
    /// </summary>
    [SerializeField] private float explodeDelay = 5 / 6;
    
    /// <summary>
    /// Rigidbodyが保存されてる変数
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// ヒットしたかどうか
    /// </summary>
    private bool isCollisionEntered = false;

    /// <summary>
    /// <see cref="destroyParticle"/>の動作時間
    /// </summary>
    private float destroyTime = 0;

    /// <summary>
    /// 時間カウント用変数
    /// </summary>
    private float t=0;

    /// <summary>
    /// サイズ変更時の元のサイズ
    /// </summary>
    private Vector3 defaultScale;
    

	// Use this for initialization
	void Start ()
    {
        //transform.Rotate(rotX, rotY, rotZ);
        rb = GetComponent<Rigidbody>();
        destroyTime = destroyParticle.main.duration;
        rb.AddForce(transform.forward * 50 * moveSpeed, ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isCollisionEntered)
        {
            //transform.Translate(0, 0, 1 * moveSpeed);
            rb.AddForce(transform.forward*10*moveSpeed);
        }
        else
        {
            t += Time.deltaTime;
            float sc = (1 - t / destroyTime);
            //transform.localScale = defaultScale * (sc);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollisionEntered)
        {
            Hit();
            var otherObj = collision.gameObject;
            if (otherObj.tag == "Character")
            {
                otherObj.GetComponent<Character>().explodeDamage(explodeDamageValue);
            }
        }
    }

    /// <summary>
    /// ヒットした時の処理
    /// </summary>
    public void Hit()
    {
        isCollisionEntered = true;
        var particle = Instantiate(destroyParticle, transform);
        particle.Play();
        Destroy(gameObject, destroyTime+(explodeDelay));
        rb.isKinematic = true;
        defaultScale = transform.localScale;
        var collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size *= 5;
        
    }

    private void OnTriggerEnter(Collider other)
    {

        var otherObj = other.gameObject;
        if (otherObj.tag == "Character")
        {
            otherObj.GetComponent<Character>().explodeDamage(explodeDamageValue*0.8f);
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
