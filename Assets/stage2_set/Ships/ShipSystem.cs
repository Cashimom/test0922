using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// モンスターをたくさんスポーンするシステム
/// </summary>
public class ShipSystem : MonoBehaviour
{
    [SerializeField] public List<Transform> SpawnPositions;

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
        if (SpawnPositions.Count == 0) SpawnPositions.Add(transform);
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
                if (enemies[i] == null||enemies[i].gameObject.activeSelf==false)
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
        //Transform spawnPos = SpawnPositions[UnityEngine.Random.Range(0, SpawnPositions.Count)];
        Transform spawnPos = SpawnPositions[(int)((float)spawnCnt/( (float)MaxSpawn / (float)SpawnPositions.Count))];
        if (spawnPos == null) spawnPos = transform;
        var monster = Instantiate(Monster1, 
            position: spawnPos.position - spawnPos.up*10+ spawnPos.right*UnityEngine.Random.Range(-50,50)+ spawnPos.forward * UnityEngine.Random.Range(-51, 51),
            rotation: spawnPos.rotation);
        var eneCon = monster.GetComponent<EnemyController>();
        var weapon = Instantiate(Weapon1).GetComponent<Weapon>();
        weapon.character = eneCon;
        weapon.WeaponTransformDistance = 11;
        eneCon.RightWeapon = weapon;
        eneCon.target = target;
        enemies.Add(eneCon);
        spawnCnt++;
        return eneCon;
    }

    private void OnDestroy()
    {
        SpawnPositions.Clear();
    }

}
