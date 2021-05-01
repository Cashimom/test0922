using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

//クリアの判定など
public class GameSystem : MonoBehaviour
{
    //プレイヤー
    [SerializeField] public GameObject Player;

    //プレイヤーのリスポーン場所
    [SerializeField] public Vector3 SpawnPosition;

    //クリアの判定をするship
    [SerializeField] public List<ShipSystem> shipSystems;

    /// <summary>
    /// <see cref="Player"/>のplayerController
    /// </summary>
    private PlayerController playerController;

    private float clearCnt = 0;


    // Start is called before the first frame update
    void Awake()
    {
        if (Player == null)
        {
            Debug.LogError("Playerが設定されていない");
            //Application.Quit();
        }
        playerController = Player.GetComponent<PlayerController>();
        if (SpawnPosition == new Vector3(0,0,0))
        {
            SpawnPosition = Player.transform.position;
        }
        playerController.dieFunc += dieFunc;
    }

    private void OnDisable()
    {
        shipSystems.ForEach(ship =>
        {
            if(ship!=null)
                ship.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        shipSystems.ForEach(ship =>
        {
            if(ship.gameObject!=null)
                ship.gameObject.SetActive(true);
        });
    }

    void FixedUpdate()
    {
        bool flg = false;
        shipSystems.ForEach((ship) =>
        {
            if ((ship != null && !ship.AllFinish))
            {
                flg = true;
            }
        });
        if (!flg)
        {
            GameClear();
        }
    }

    void dieFunc()
    {

        Player.transform.position=SpawnPosition;
        playerController.HP = playerController.MaxHP;
    }
    
    void GameClear()
    {
        if (clearCnt == 0)
        {
            var tmp_ = GameObject.Find("Canvas/Center Text");
            var tmp = tmp_.GetComponent<TextMeshProUGUI>();
            tmp.enabled = true;
            tmp.text = "Game Clear!";
        }
        clearCnt += Time.deltaTime;
        if (clearCnt > 5)
        {
            var tmp = GameObject.Find("Canvas/Center Text").GetComponent<TextMeshProUGUI>();
            tmp.text ="After "+ (15 - (int)clearCnt).ToString() + " seconds\nyou will return to Stage_Select_World";
        }
        if (clearCnt > 15)
        {
            SceneManager.LoadScene("StageSelectScene");
        }

    }
}
