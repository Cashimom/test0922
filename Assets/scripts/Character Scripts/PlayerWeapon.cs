using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Players
{
    public class PlayerWeapon: MonoBehaviour
    {
        [SerializeField] public PlayerController2 playerController2;

        // 持っている武器
        [SerializeField] public List<Weapon> WeaponList;

        // 持っている武器
        [SerializeField] public Weapon RightWeapon;

        // 持っている武器2
        [SerializeField] public Weapon LeftWeapon;

        public const int WEAPON_RIGHT = 1;
        public const int WEAPON_LEFT = 2;
        public int NowWeapon = WEAPON_RIGHT;

        private Weapon nearWeapon;
        /// <summary>
        /// 拾うことができる近くのアイテム。
        /// setterで<see cref="pressButton"/>を切り替え。
        /// set in <see cref="Weapon.OnTriggerEnter(Collider)"/>
        ///  and <see cref="Weapon.OnTriggerExit(Collider)"/>
        /// </summary>
        public Weapon NearWeapon
        {
            set
            {
                this.nearWeapon = value;
                //TODO:
                /*
                if (value != null)
                {

                    if (pressButton != null)
                    {
                        pressButton.SetActive(true);
                        pressButton.transform.Find("Weapon Image").GetComponent<RawImage>().texture = nearWeapon.image;
                    }
                }
                else
                {
                    if (pressButton != null)
                    {
                        pressButton.SetActive(false);
                    }
                }
                */

            }
            get
            {
                return this.nearWeapon;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        //
        public void Fire1()
        {
            RightWeapon.Fire1();
        }

        //
        public void Fire2()
        {

        }



        /// <summary>
        /// <paramref name="picked"/>を拾う
        /// </summary>
        /// <param name="picked">拾うWeapon</param>
        public void WeaponPickUp(Weapon picked)
        {

            //枠がいっぱいなら手持ちと交換
            if (WeaponList.Count >= 2)
            {
                int index = WeaponList.FindIndex(match => RightWeapon == match);
                WeaponList[index] = picked;
                RightWeapon.DropWeapon();
                picked.HaveWeapon(playerController2);
                RightWeapon = picked;
            }
            //何も持ってなかったらすぐ装備
            else if (RightWeapon == null && WeaponList.Count < 2)
            {
                WeaponList.Add(picked);
                picked.HaveWeapon(playerController2);
                RightWeapon = picked;
                if (LeftWeapon != null)
                    LeftWeapon.isHave = false;
            }
            //何か持ってて枠が空いてたら装備せずに拾う
            else if (RightWeapon != null && WeaponList.Count < 2)
            {
                WeaponList.Add(picked);
                picked.PickWeapon(playerController2);
            }

            // TODO: uiの処理をまた別に分離する
            // uiController.SlotUpdate(WeaponList);
            // uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
        }

        /// <summary>
        /// 武器を持ち替える
        /// </summary>
        /// <param name="sign">持ち替える方向(正負)</param>
        public void WeaponChange(int sign)
        {
            if (NowWeapon == WEAPON_LEFT)
            {
                WeaponSwitch();
            }
            var nowIndex = WeaponList.FindIndex(match => match == RightWeapon);
            if (WeaponList.Count >= 2)
            {
                Weapon will;
                if (sign > 0 && nowIndex + 1 >= WeaponList.Count)
                {
                    will = WeaponList[0];
                }
                else if (sign > 0 && nowIndex + 1 < WeaponList.Count)
                {
                    will = WeaponList[nowIndex + 1];
                }
                else if (sign < 0 && nowIndex <= 0)
                {
                    will = WeaponList[WeaponList.Count - 1];
                }
                else if (sign < 0 && nowIndex > 0)
                {
                    will = WeaponList[nowIndex - 1];
                }
                else
                {
                    will = WeaponList[0];
                }
                //debugText(WeaponList[nowIndex].ToString() +":::"+ Time.time.ToString());
                RightWeapon.ChangeWeapon();
                will.HaveWeapon(playerController2);
                RightWeapon = will;
            }

            // TODO:
            // uiController.SlotUpdate(WeaponList);
            // uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
        }


        public void WeaponSwitch()
        {
            if (NowWeapon == WEAPON_RIGHT && LeftWeapon != null)
            {
                NowWeapon = WEAPON_LEFT;
                if (RightWeapon != null)
                    RightWeapon.isHave = false;
                LeftWeapon.isHave = true;
                // TODO:
                //uiController.SetActiveSlot(2);
            }
            else if (NowWeapon == WEAPON_LEFT && RightWeapon != null)
            {
                NowWeapon = WEAPON_RIGHT;
                RightWeapon.isHave = true;
                if (LeftWeapon != null)
                    LeftWeapon.isHave = false;
                //TODO:
                //uiController.SetActiveSlot(WeaponList.FindIndex(m => m == RightWeapon));
            }
        }
    }
}