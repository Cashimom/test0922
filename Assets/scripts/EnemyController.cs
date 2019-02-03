using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character {

    //[SerializeField] private float moveSpeed = 5.0f;
    public float cycle = 10.0f;
    private float time = 0.0f;
    private int wayFlg = 0;
    public Vector3 moveVec=new Vector3(1,0,0);
    //public float HP = 100;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= cycle)
        {
            time = 0.0f;
            wayFlg = 1-wayFlg;
        }
        if (wayFlg == 0)
        {
            move(moveVec, 1);
        }
        else if (wayFlg == 1)
        {
            move(-moveVec, 1);
        }

	}

    public override bool explodeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            die();
        }
        return true;
    }


}
