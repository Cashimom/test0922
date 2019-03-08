using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameSystem : MonoBehaviour
{
    [SerializeField] public GameObject Player;
    [SerializeField] public Vector3 SpawnPosition;
    [SerializeField] public List<ShipSystem> shipSystems;
    private playerController playerController;

    private float clearCnt = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
        {
            Debug.LogError("Playerが設定されていない");
            Application.Quit();
        }
        playerController = Player.GetComponent<playerController>();
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
            if(ship.gameObject!=null)
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

    // Update is called once per frame
    void Update()
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
        playerController.HP = 100;
        var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        tmp.text = playerController.HP.ToString();
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
