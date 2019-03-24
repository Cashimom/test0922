using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 爆発によるダメージの処理をする。
    /// see <see cref="Character.explodeDamage(float)"/>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    /// <returns></returns>
    public override bool explodeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            die();
        }
        return true;
    }

    public override bool explodeDamage(float damage,Character character)
    {
        HP -= damage ;
        if (HP <= 0)
        {
            die();
        }
        return true;
    }
}
