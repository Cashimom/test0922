using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character {

    //[SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float cycle = 10.0f;
    [SerializeField] private Vector3 moveVec=new Vector3(1,0,0);
    [SerializeField] private GameObject target;
    [SerializeField] private int delay = 2;
    private float time = 0.0f;
    private int wayFlg = 0;
    private int delayCnt = 0;
    //public float HP = 100;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rightWeaponTransform = transform;
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

        if (target != null)
        {
            if (Weapon != null)
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - Weapon.ShotTransform.position);

            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            }
        }

        if(Weapon != null)
        {

            delayCnt++;
            if (delayCnt >= delay)
            {
                delayCnt = 0;
                Weapon.Fire1();
            }
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
