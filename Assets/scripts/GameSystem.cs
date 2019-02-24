using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSystem : MonoBehaviour
{
    [SerializeField] public GameObject Player;
    [SerializeField] public Vector3 SpawnPosition;
    private playerController playerController;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void dieFunc()
    {

        Player.transform.position=SpawnPosition;
        playerController.HP = 100;
        var tmp = GameObject.Find("Canvas/ShowEnergy Text2").GetComponent<TextMeshProUGUI>();
        tmp.text = "HP : " + playerController.HP.ToString();
    }
}
