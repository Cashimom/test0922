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
                //extUpdate(Mathf.RoundToInt(camera.pixelHeight));
            }
            if (d > camera.fieldOfView/180&&new Rect(0,0,1,1).Contains(rectPos))
            {
                rectPos.x *= camera.pixelWidth;
                rectPos.y *= camera.pixelHeight;
                pointers[i].transform.position = rectPos;
            }
            else
            {
                var x = rectPos.x;
                var y = rectPos.y;
                if (d > 0)
                {
                    x = Mathf.Clamp01(x) * camera.pixelWidth;
                    y = Mathf.Clamp01(y) * camera.pixelHeight;
                }
                else
                {
                    x = (Mathf.Clamp01(x) - 0.5f < 0 ? 1 : 0) * camera.pixelWidth;
                    y = (1-Mathf.Clamp01(y)) * camera.pixelHeight;
                }
                /*
                if (0<=x&&x <= 1&&(y<0||y>1)&&d>0)
                {
                    rectPos.x *= camera.pixelWidth;
                    rectPos.y *= Mathf.Clamp01(rectPos.y)* camera.pixelHeight;
                    pointers[i].transform.position = rectPos;
                }
                else if (0 <= y && y <= 1 && (x < 0 || x > 1)&&d>0)
                {
                    rectPos.y *= camera.pixelHeight;
                    rectPos.x *= Mathf.Clamp01(rectPos.x) * camera.pixelWidth ;
                    pointers[i].transform.position = rectPos;
                }
                else
                {
                    rectPos.x = camera.pixelWidth;
                    rectPos.y = camera.pixelHeight;
                    pointers[i].transform.position = rectPos;
                }*/
                rectPos.x = Mathf.Lerp(pointers[i].transform.position.x, x, 0.5f);
                rectPos.y = Mathf.Lerp(pointers[i].transform.position.y, y, 0.5f); ;
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
