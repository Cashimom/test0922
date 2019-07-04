using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ターゲットのカウントをしてるUIを動かすスクリプト
/// </summary>
public class TargetCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valueText;

    [SerializeField] TextMeshProUGUI maxValueText;

    [SerializeField] int value = 0;

    [SerializeField] int maxValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        textUpdate(value, maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void textUpdate()
    {
        valueText.text = value.ToString();
    }
    public void textUpdate(int v)
    {
        value = v;
        valueText.text = value.ToString();
    }
    public void textUpdate(int v,int maxVal)
    {
        value = v;
        valueText.text = value.ToString();
        maxValue=maxVal;
        maxValueText.text = maxValue.ToString();
    }
}
