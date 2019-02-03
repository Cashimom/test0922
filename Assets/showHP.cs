using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class showHP : MonoBehaviour {

    TextMeshProUGUI tmp;
    private Text targetText;
    public Character character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //targetText = this.GetComponent<Text>();
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = character.HP.ToString();
	}
}
