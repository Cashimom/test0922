using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSystem : MonoBehaviour
{

    //[SerializeField] public List<GameObject> monsterList;
    [SerializeField] private GameObject Monster1;

    [SerializeField] private GameObject Weapon1;

    [SerializeField] public GameObject target;

    private float spawnTimeCnt = 0;

    private int spawnCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCnt<100)spawnTimeCnt += Time.deltaTime;
        if (spawnTimeCnt > 5&&spawnCnt<100)
        {
            spawnTimeCnt = 0;
            SpawnMonster1();
            spawnCnt++;
        }
    }

    public EnemyController SpawnMonster1()
    {
        var monster = Instantiate(Monster1, transform.position - transform.up*100, transform.rotation);
        var eneCon = monster.GetComponent<EnemyController>();
        var weapon = Instantiate(Weapon1).GetComponent<Weapon>();
        weapon.character = eneCon;
        weapon.WeaponTransformDistance = 11;
        eneCon.Weapon = weapon;
        eneCon.target = target;
        return eneCon;
    }

}
