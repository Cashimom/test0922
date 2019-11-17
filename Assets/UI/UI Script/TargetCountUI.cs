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
        //textUpdate(value, maxValue);
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

    /// <summary>
    /// ターゲットをロックオンするやつ
    /// </summary>
    public void TargetPointerUpdate()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                pointers[i].enabled = false;
                continue;
            }

            ///ワールド座標を画面座標(0-1)に変換
            var rectPos = camera.WorldToViewportPoint(targets[i].transform.position);
            rectPos.z = 0;
            //カメラからターゲットまでのベクトルと、カメラの正面のベクトルの内積
            var d = Vector3.Dot((targets[i].transform.position - camera.transform.position).normalized, camera.transform.forward);


            //ターゲットが画面内なら
            if (d > Mathf.Cos(camera.fieldOfView/2)&&new Rect(0,0,1,1).Contains(rectPos))
            {
                rectPos.x *= camera.pixelWidth;
                rectPos.y *= camera.pixelHeight;
                pointers[i].transform.position = rectPos;
            }
            ///画面外
            else
            {
                var x = rectPos.x;
                var y = rectPos.y;
                ///前なら0か1に
                if (d > 0)
                {
                    x = Mathf.Clamp01(x) * camera.pixelWidth;
                    y = Mathf.Clamp01(y) * camera.pixelHeight;
                }
                ///なぜか後ろだとターゲットの座標が本来の反対にあるようになるので加工
                else
                {
                    x = (Mathf.Clamp01(x) - 0.5f < 0 ? 1 : 0) * camera.pixelWidth;
                    y = (1-Mathf.Clamp01(y)) * camera.pixelHeight;
                }
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
