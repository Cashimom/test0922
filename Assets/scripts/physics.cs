﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics : MonoBehaviour {

    [SerializeField] private Vector3 localGravity;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {

        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;

    }

    // Update is called once per frame
    void Update()
    {


    }

    void FixedUpdate()
    {
        setLocalGravity();
    }

    void setLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }

}
