using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ターゲットのカウントをしてるUIを動かすスクリプト
/// </summary>
public class TargetCountUI : MonoBehaviour
{

    [SerializeField] RawImage pointerPrefab;

    [SerializeField] TextMeshProUGUI valueText;

    [SerializeField] TextMeshProUGUI maxValueText;

    [SerializeField] public List<GameObject> targets;

    public Camera camera;

    [SerializeField] int value = 0;

    [SerializeField] int maxValue = 0;

    public List<RawImage> pointers=new List<RawImage>();

    // Start is called before the first frame update
    void Start()
    {
        textUpdate(value, maxValue);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TargetPointerUpdate();
    }

    public void TargetPointerStart(List<GameObject> targets,Camera camera)
    {
        this.targets = targets;
        this.camera = camera;
        for(int i=0; i < targets.Count; i++)
        {
            //Debug.Log(i);
            var ins = Instantiate(pointerPrefab);
            pointers.Add(ins);
            ins.transform.parent = this.gameObject.transform;
        }

    }

    public void TargetPointerUpdate()
    {
        var cameraDir = camera.transform.forward;
        for (int i = 0; i < targets.Count; i++)
        {
            var rectPos = camera.WorldToViewportPoint(targets[i].transform.position);
            var d = Vector3.Dot((targets[i].transform.position - camera.transform.position).normalized, (cameraDir));
            
            if (i == 0)
            {
                textUpdate(Mathf.FloorToInt(d * 10));
            }
            if (d > 0.5)
            {
                rectPos.x *= camera.pixelWidth;
                rectPos.y *= camera.pixelHeight;
                //Debug.Log(rectPos);
                pointers[i].transform.position = rectPos;
            }
            
        }


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
