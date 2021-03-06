﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 何かを倒す(破壊)すると隠し武器出現するシステム
/// </summary>
public class BonusSystem : MonoBehaviour
{
    /// <summary>
    /// 倒す敵
    /// </summary>
    [SerializeField] public List<Character> enemies;

    /// <summary>
    /// 隠し武器
    /// </summary>
    [SerializeField] private GameObject DropWeapon;

    /// <summary>
    /// 隠し武器
    /// </summary>
    [SerializeField] private ParticleSystem particle;

    [SerializeField] private bool notPlayerKill=false;

    // Start is called before the first frame update
    void Start()
    {
        DropWeapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool flg = false;
        enemies.ForEach((charcter) =>
        {
            if (((charcter==null&&notPlayerKill)|| 
            (charcter!=null && charcter.gameObject.activeSelf==true&&!notPlayerKill) || 
            (notPlayerKill && charcter != null &&( (EnemyController)charcter).killedByNotPlayer==false)))
                flg = true;
        });
        if (!flg)
        {
            //Debug.Break();
            DropWeapon.SetActive(true);
            this.enabled = false;
            if (particle != null)
            {
                var parti = Instantiate(particle, DropWeapon.transform);
                parti.transform.position = DropWeapon.transform.position;
                parti.Play();
            }
        }

    }
}
