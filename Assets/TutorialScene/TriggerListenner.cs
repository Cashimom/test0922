using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerListenner : MonoBehaviour
{
    [SerializeField] public Action<Collider> action;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (action != null)
            action(other);
    }
}
