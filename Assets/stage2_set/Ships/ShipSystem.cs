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
    /// モンスターをスポーンさせる場所
    /// </summary>
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

    [SerializeField] public List<SavingObject> targetObjects;

    /// <summary>
    /// ステージを生成するやつ
    /// </summary>
    [SerializeField] public StageGenerator generator;


    /// <summary>
    /// <see cref="this"/>からスポーンしたすべてのモンスターが倒されたかどうか
    /// </summary>
    [NonSerialized] public bool AllFinish = false;

    /// <summary>
    /// 残ってる<see cref="targetObjects"/>"の数
    /// </summary>
    [NonSerialized] public int targetCount = 0;

    /// <summary>
    /// 時間カウント用
    /// </summary>
    private float spawnTimeCnt = 0;

    /// <summary>
    /// スポーンしたモンスターの数
    /// </summary>
    private int spawnCnt = 0;

    private UIController uiController;
    

    /// <summary>
    /// スポーンしたモンスターの情報を保持するリスト
    /// </summary>
    private List<EnemyController> enemies=new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        if (SpawnPositions.Count == 0) SpawnPositions.Add(transform);
        uiController = GameObject.Find("Canvas").GetComponent<UIController>();
        List<GameObject> a = new List<GameObject>();
        targetObjects.AddRange(generator.generate());
        foreach (var i in targetObjects)
        {
            a.Add(i.savingObject);
        }
        uiController.setTargetPointer(a);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(spawnCnt<MaxSpawn)spawnTimeCnt += Time.deltaTime;
        var pmax = (int)(((float)MaxSpawn / (float)SpawnPositions.Count) * Math.Floor(((float)spawnCnt-0.1f) / ((float)MaxSpawn / (float)SpawnPositions.Count) + 1));
        if (pmax > MaxSpawn) pmax = MaxSpawn;
        //GetComponent<EnemyController>().debugText(spawnCnt.ToString() + " / " + pmax.ToString());
        if (spawnTimeCnt > SpawnTime&&spawnCnt<pmax|| spawnCnt >= pmax&&enemies.Count==0&&spawnCnt<MaxSpawn)
        {
            spawnTimeCnt = 0;
            SpawnMonster1();
        }
        
        if (spawnCnt >= pmax)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null||enemies[i].gameObject.activeSelf==false)
                {
                    enemies.Remove(enemies[i]);
                    i--;
                }
            }
            if (enemies.Count == 0&& spawnCnt>=MaxSpawn)
            {

                AllFinish = true;
            }
        }

        var targetsDestroy = 0;
        foreach(var to in targetObjects)
        {
            if (to.savingObject!=null&& to.savingObject.activeSelf)
            {
                targetsDestroy++;
            }
        }
        if (targetsDestroy==0)
        {
            AllFinish = true;
        }
        if(targetCount!= targetsDestroy)
        {
            uiController.setTargetCount(targetObjects.Count-targetsDestroy, targetObjects.Count);
        }
        targetCount = targetsDestroy;
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
        var weap = Instantiate(Weapon1);
        weap.layer = 12;
        foreach(Transform o in weap.transform)
        {
            o.gameObject.layer = 12;
        }
        var weapon = weap.GetComponent<Weapon>();
        weapon.character = eneCon;
        //weapon.WeaponTransformDistance = 11;
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
