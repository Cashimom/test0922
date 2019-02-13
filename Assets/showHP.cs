using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//prefab "HP" 専用
//PlayerのHPを 3D Objectで表示
public class showHP : MonoBehaviour
{
    private int maxHitPoint = 100;
    private int currentHitPoint;
    private int fragmentNum = 4;
    private bool isDead = false;

    void Start()
    {
        currentHitPoint = maxHitPoint;
    }

    // 残りHPの割合に応じて 子Object(HPの欠片)のScaleを変更
    void Update()
    {
        //for(HPの欠片の数)
        for (int i = 1;i <= fragmentNum;i++)
        {
            //null対策がぬるいかも
            if (transform.GetChild(i-1) != null)
            {
                //欠片一つが保持しているHP
                float fragmentHP = maxHitPoint / fragmentNum;
                //受けたダメージ
                float recievedHP = maxHitPoint - currentHitPoint;

                //i番目の欠片のHPより,受けたダメージが大きい(死)
                if (fragmentHP * i <= recievedHP) {
                    //scale = 0
                    transform.GetChild(i-1).gameObject.transform.localScale = new Vector3(0, 0, 0);

                    if (i == fragmentNum)
                    {
                        isDead = true;
                    }
                }
                //欠片のHPが残っている
                else
                {
                    isDead = false;

                    //欠片の大きさを計算
                    float a = (fragmentHP*i - recievedHP) / fragmentHP;
                    transform.GetChild(i-1).gameObject.transform.localScale = new Vector3(a,a,a);

                    //後の欠片も生きてるのでbreak,さもなくば計算が崩れる
                    break;
                }

            }
        }
    }

    //現在のHPに加算
    public void AddHitPoint(int a)
    {
        if (currentHitPoint + a <= 0)
        {
            Kill();
        }
        else
        {
            currentHitPoint += a;
        }

    }
    //HPをゼロにする
    public void Kill()
    {
        currentHitPoint = 0;
        isDead = true;
    }
    //Is he dead
    public bool IsDead()
    {
        return isDead;
    }
}
