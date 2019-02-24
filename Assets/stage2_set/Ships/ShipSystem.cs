using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class ShipSystem : MonoBehaviour
{

    //[SerializeField] public List<GameObject> monsterList;
    [SerializeField] private GameObject Monster1;

    [SerializeField] private GameObject Weapon1;

    [SerializeField] public GameObject target;

    [SerializeField] private float SpawnTime = 5;

    [SerializeField] private int MaxSpawn = 100;

    private float spawnTimeCnt = 0;

    private int spawnCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCnt<MaxSpawn)spawnTimeCnt += Time.deltaTime;
        if (spawnTimeCnt > SpawnTime&&spawnCnt<MaxSpawn)
        {
            spawnTimeCnt = 0;
            SpawnMonster1();
            spawnCnt++;
        }
    }

    public EnemyController SpawnMonster1()
    {
        var monster = Instantiate(Monster1, position: transform.position - transform.up*100+transform.right*Random.Range(-200,200)+ transform.forward * Random.Range(-200, 200), rotation: transform.rotation);
        var eneCon = monster.GetComponent<EnemyController>();
        var weapon = Instantiate(Weapon1).GetComponent<Weapon>();
        weapon.character = eneCon;
        weapon.WeaponTransformDistance = 11;
        eneCon.weapon = weapon;
        eneCon.target = target;
        return eneCon;
    }

}
