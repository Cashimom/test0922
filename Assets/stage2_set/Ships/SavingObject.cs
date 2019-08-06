using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingObject : MonoBehaviour
{
    [SerializeField] public GameObject savingObject;

    [SerializeField]public GameObject _Monster;

    [SerializeField] public GameObject _Weapon;

    public GameObject _Target;

    [SerializeField] public int monsterCount = 20;

    [SerializeField] public float range = 100;

    private SphereCollider sphereCollider;

    private void Awake()
    {

        sphereCollider = gameObject.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            //sphereCollider= gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.radius = range;
    }

    void Start()
    {
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
        if (obj.tag == "Character"&& obj.layer==9/*&&obj.GetComponent<Character>() is PlayerController*/)
        {
            _Target = obj;
            StartUp();
            sphereCollider.enabled = false;
        }
    }


    void StartUp()
    {
        for(int i=0; i < monsterCount;i++)
        {
            var vec = new Vector3(UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1).normalized;
            vec *= range ;
            vec *= Random.value;

            SpawnMonster1(_Monster, _Weapon, _Target, transform.position + vec);
        }
    }

    public EnemyController SpawnMonster1(GameObject monster ,GameObject weapon ,GameObject target,Vector3 pos)
    {
        var _monster = Instantiate(monster, position: pos, rotation: Quaternion.Euler(pos));
        var eneCon = _monster.GetComponent<EnemyController>();
        var weap = Instantiate(weapon);
        //weap.layer = 12;
        //foreach (Transform o in weap.transform)
        //{
        //    o.gameObject.layer = 12;
        //}
        var _weapon = weap.GetComponent<Weapon>();
        _weapon.character = eneCon;
        //weapon.WeaponTransformDistance = 11;
        eneCon.RightWeapon = _weapon;
        eneCon.target = target;
        return eneCon;
    }
}
