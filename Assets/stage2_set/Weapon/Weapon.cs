using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 武器に継承させるクラス。
/// <seealso cref="GatlingScript"/>
/// <seealso cref="ShotRocket"/>
/// </summary>
public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 発射するロケットのゲームオブジェクト
    /// (include <see cref="RocketScript"/> )
    /// </summary>
    [SerializeField] protected GameObject rocket;

    /// <summary>
    /// <see cref="rocket"/> を発射するTransform
    /// </summary>
    [SerializeField] public Transform ShotTransform;

    /// <summary>
    /// 武器を持っている<see cref="Character"/>
    /// </summary>
    [SerializeField] public Character character;

    /// <summary>
    /// "Press F"って表示させるためのテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI pressButton;

    /// <summary>
    /// エネミーが武器持つときにを浮かせる位置
    /// </summary>
    [SerializeField] public float WeaponTransformDistance = 8;

    /// <summary>
    /// <see cref="character"/>が持っているかどうか。
    /// (character!=nullでよくね、そのうち消す)
    /// </summary>
    protected bool isHave = false;

    /// <summary>
    /// 近くにCharacterがいるかどうか。
    /// Changed by <seealso cref="Weapon.OnTriggerEnter(Collider)"/>
    /// or <seealso cref="Weapon.OnTriggerExit(Collider)"/>
    /// </summary>
    protected bool near = false;

    /// <summary>
    /// <see cref="character"/>がプレイヤーかどうか
    /// </summary>
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
         終わり
         
    */

    // Use this for initialization
    public void Start () {
        if (character is playerController)
        {
            isPlayer = true;
        }

        if (pressButton == null)
        {
            pressButton= GameObject.Find("Canvas/PressButton Text").GetComponent<TextMeshProUGUI>();
        }

        isHave = (character != null);
    }
	
	// Update is called once per frame
	public void Update () {
        if (near&&(!isHave))
        {
            if (Input.GetKeyDown("f"))
            {
                isPlayer = (character is playerController);
                character.Weapon.DropWeapon();
                HaveWeapon();
                near = false;
                pressButton.enabled = true;
                character.Weapon = this;
            }
        }
	}

    /// <summary>
    /// <see cref="rocket"/>を<see cref="ShotTransform"/>に生成する
    /// </summary>
    /// <returns>
    /// 生成したロケットのゲームオブジェクト
    /// </returns>
    public RocketScript Fire()
    {
        var fire = Instantiate(rocket, ShotTransform.position + ShotTransform.forward * 2, ShotTransform.rotation);
        return fire.GetComponent<RocketScript>();
    }

    /// <summary>
    /// Fire1 (左クリック)が押されているときの処理を書く
    /// </summary>
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

    /// <summary>
    /// キャラクターから<see cref="this"/>をドロップする
    /// </summary>
    public void DropWeapon()
    {
        isHave = false;
        character = null;
        GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// キャラクターに<see cref="this"/>を持たせる
    /// </summary>
    public void HaveWeapon()
    {
        isHave = true;
        GetComponent<BoxCollider>().enabled = false;
    }
}
