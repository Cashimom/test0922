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
    /// エネミーが武器持つときにを浮かせる位置
    /// </summary>
    [SerializeField] public float WeaponTransformDistance = 8;

    /// <summary>
    /// 弾を撃つ間隔
    /// </summary>
    [SerializeField] protected float fireTick = 0.1f;

    [SerializeField] public Texture image;

    /// <summary>
    /// <see cref="character"/>が持っているかどうか。
    /// 
    /// </summary>
    protected bool isHave = false;

    /// <summary>
    /// <see cref="character"/>がプレイヤーかどうか
    /// </summary>
    protected bool isPlayer = false;

    /// <summary>
    /// 時間をカウントする変数
    /// </summary>
    protected float fireTime = 0.0f;

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

        isHave = (character != null);
    }
	
	// Update is called once per frame
	public void Update () {

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
        if(character is playerController)fire.layer = 11;
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
        var pc = other.gameObject.GetComponent<playerController>();
        if (pc != null&&!isHave)
        {
            pc.NearWeapon = this;
            character = pc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pc = other.gameObject.GetComponent<playerController>();
        if (pc != null && !isHave) 
        {
            pc.NearWeapon = null;
            //character = null;
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
    /// キャラクターに<see cref="this"/>を拾わせる
    /// </summary>
    public void PickWeapon()
    {

        isPlayer = (character is playerController);
        GetComponent<BoxCollider>().enabled = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// キャラクターに<see cref="this"/>を持たせる
    /// </summary>
    public void HaveWeapon()
    {
        PickWeapon();
        isHave = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// <see cref="this"/>を他のWeaponに持ち帰る
    /// </summary>
    public void ChangeWeapon()
    {
        isHave = false;
        gameObject.SetActive(false);
    }
}
