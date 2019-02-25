﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSystem : MonoBehaviour
{
    [SerializeField] public List<Character> enemies;

    [SerializeField] private GameObject DropWeapon;

    [SerializeField] private ParticleSystem particle;

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
            if (charcter != null)
                flg = true;
        });
        if (!flg)
        {
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
