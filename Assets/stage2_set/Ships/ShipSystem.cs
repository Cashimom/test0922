using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// モンスターをたくさんスポーンするシステム
/// </summary>
public class ShipSystem : MonoBehaviour
{

    /// <summary>
    /// スポーンするモンスター
    /// </summary>
    [SerializeField] private GameObject Monster1;

    /// <summary>
    /// <see cref="Monster1"/>に持たせる武器
    /// </summary>
    [SerializeField] private GameObject Weapon1;

    /// <summary>
    /// モンスターに襲わせるターゲット
    /// </summary>
    [SerializeField] public GameObject target;

    /// <summary>
    /// モンスターがスポーンする時間間隔
    /// </summary>
    [SerializeField] private float SpawnTime = 5;

    /// <summary>
    /// モンスターをスポーンさせる最大個数
    /// </summary>
    [SerializeField] private int MaxSpawn = 100;

    /// <summary>
    /// <see cref="this"/>からスポーンしたすべてのモンスターが倒されたかどうか
    /// </summary>
    [NonSerialized] public bool AllFinish = false;

    /// <summary>
    /// 時間カウント用
    /// </summary>
    private float spawnTimeCnt = 0;

    /// <summary>
    /// スポーンしたモンスターの数
    /// </summary>
    private int spawnCnt = 0;

    /// <summary>
    /// スポーンしたモンスターの情報を保持するリスト
    /// </summary>
    private List<EnemyController> enemies=new List<EnemyController>();

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
        }
        if (spawnCnt == MaxSpawn)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.Remove(enemies[i]);
                    i--;
                }
            }
            if (enemies.Count == 0)
            {

                AllFinish = true;
            }
        }
    }

    public EnemyController SpawnMonster1()
    {
        var monster = Instantiate(Monster1, position: transform.position - transform.up*100+transform.right*UnityEngine.Random.Range(-200,200)+ transform.forward * UnityEngine.Random.Range(-200, 200), rotation: transform.rotation);
        var eneCon = monster.GetComponent<EnemyController>();
        var weapon = Instantiate(Weapon1).GetComponent<Weapon>();
        weapon.character = eneCon;
        weapon.WeaponTransformDistance = 11;
        eneCon.weapon = weapon;
        eneCon.target = target;
        enemies.Add(eneCon);
        spawnCnt++;
        return eneCon;
    }

    

}
