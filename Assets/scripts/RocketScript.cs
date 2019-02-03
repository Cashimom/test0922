﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RocketScript : MonoBehaviour {

    public float moveSpeed = 1.0f;
    //public float rotY = 0.0f, rotX = 0.0f, rotZ = 0.0f;
    public ParticleSystem destroyParticle;
    private Rigidbody rb;
    private bool isCollisionEntered = false;
    private float destroyTime = 0;
    private float t=0;
    private Vector3 defaultScale;
    public float explodeDamageValue = 10.0f;
    public float explodeDelay = 5 / 6;
    //public showHP showHP;
    public TextMeshProUGUI tmp;
    

	// Use this for initialization
	void Start ()
    { 
        //transform.Rotate(rotX, rotY, rotZ);
        rb = GetComponent<Rigidbody>();
        destroyTime = destroyParticle.main.duration;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!isCollisionEntered)
            transform.Translate(0, 0, 1 * moveSpeed);
        else
        {
            t += Time.deltaTime;
            float sc= (1 - t / destroyTime);
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
                tmp = GameObject.Find("Canvas/showHP Text").GetComponent<TextMeshProUGUI>();
                tmp.GetComponent<showHP>().character = otherObj.GetComponent<Character>();

            }
        }
    }

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
        collider.size = new Vector3(5, 5, 5);
        
    }

    private void OnTriggerEnter(Collider other)
    {
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
