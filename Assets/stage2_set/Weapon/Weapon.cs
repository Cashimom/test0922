using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject rocket;
    [SerializeField] public Transform ShotTransform;
    [SerializeField] public Character character;
    [SerializeField] private TextMeshProUGUI pressButton;
    public bool isHave = false;
    protected bool near = false;
    protected bool isPlayer = false;

    /*
     エネミーに武器を持たせる方法
         .3Dオブジェクトをどっかに置く
         .tagをUntagからCharacterにする
         .EnemyControllerをadd Component
         .HPとかプロパティをいじくる

         .stage2_set -> Weapon　から武器のプレハブをインスタンス化する
         .

         .武器を持たせたいEnemyControllerのweaponに武器のオブジェクトをD&D
         .EnemyControllerのtargetにプレイヤーのインスタンスをD&D

         .武器のオブジェクトのcharacterにさっきのEnemyをD&D
         .武器のisHaveをにチェック
         終わり
         
    */

    // Use this for initialization
    public void Start () {
        if (character is playerController)
        {
            isPlayer = true;
        }

        isHave = (character != null);
    }
	
	// Update is called once per frame
	public void Update () {
        if (near&&(!isHave))
        {
            if (Input.GetKeyDown("f"))
            {
                if(character is playerController)
                {
                    isPlayer = true;
                }
                character.Weapon.DropWeapon();
                HaveWeapon();
                near = false;
                pressButton.enabled = true;
                character.Weapon = this;
            }
        }
	}

    public RocketScript Fire()
    {
        var fire = Instantiate(rocket, ShotTransform.position + ShotTransform.forward * 2, ShotTransform.rotation);
        return fire.GetComponent<RocketScript>();
    }

    public virtual void Fire1()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character"&&!isHave)
        {
            near = true;
            character = other.gameObject.GetComponent<Character>();
            pressButton.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character"&& !isHave)
        {
            near = false;
            //character = null;
            pressButton.enabled = false;
        }
    }

    public void DropWeapon()
    {
        isHave = false;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void HaveWeapon()
    {
        isHave = true;
        GetComponent<BoxCollider>().enabled = false;
    }
}
