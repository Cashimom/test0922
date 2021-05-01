using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListChild : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI nameText;

    [SerializeField] public TextMeshProUGUI valueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setState(string name,float value)
    {
        nameText.text = name;
        valueText.text = value.ToString(".1");
    }

    public void setState(string name, string value)
    {
        nameText.text = name;
        valueText.text = value;
    }
}
