using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class showHP : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI tmp;
    [NonSerialized] public Character character;
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
            tmp.text = character.HP.ToString();
        }
	}
}
