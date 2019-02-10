using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class showHP : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI tmp;
    [NonSerialized] public Character character;
    /////[NonSerialized] public Character character;
    private Text targetText;

    // Use this for initialization
    void Start () {
        if (tmp == null) ;
    }
	
	// Update is called once per frame
	void Update () {
        //targetText = this.GetComponent<Text>();

        if (character != null)
        {
            tmp = GetComponent<TextMeshProUGUI>();

            //HPを|||縦棒で表示. 100以上は | =50,100以下は | =5
            tmp.text = "";
            if (character.HP >= 100){
                for(int i = 0;i <= character.HP / 50; i++)
                {
                    tmp.text += "|";
                }
            }
            else if (character.HP>0)
            {
                for (int i = 0; i <= character.HP / 5; i++)
                {
                    tmp.text += "|";
                }
            }
            else
            {
                tmp.text = "-----";
            }
            tmp.text += character.name;

        }
	}
}
