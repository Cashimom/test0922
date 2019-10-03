using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    [SerializeField] private Camera camera;
    [SerializeField] private Transform head;

    [SerializeField] private List<RawImage> slotImages;
    [SerializeField] private List<Image> slotPanels;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private Slider GrenadeSlider;
    [SerializeField] private TextMeshProUGUI aiming;
    [SerializeField] private TargetCountUI targetCountUI;

    public PlayerController playerInventory;

    private Color damageColor = new Color((float)0xe5/255f, (float)0x39 /255f, (float)0x35 /255f);

    // Start is called before the first frame update
    void Start()
    {
        slotPanels.ForEach(p =>
        {
            p.color = new Color(0.3f, 0.3f, 0.3f);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        AimingUpdate();
    }

    void AimingUpdate()
    {
        if (camera == null || head == null)
        {
            return;
            //this.enabled = false;
        }
        var rect = aiming.GetComponent<RectTransform>();
        Vector3 pos = head.transform.position;
        //Vector3 pos = player.transform.position;
        pos += (new Vector3(0, -1, 0)) + head.transform.forward * 100;
        Vector3 pos_s = RectTransformUtility.WorldToScreenPoint(camera, pos);
        pos_s.y -= 10;
        if ((pos_s - rect.position).magnitude < 100)
            rect.position = Vector3.Lerp(rect.position, pos_s, 0.05f);

    }


    public void SlotUpdate(List<Weapon> weapons)
    {
        for(int i = 0; i < slotImages.Count-1; i++)
        {
            if (i < weapons.Count)
            {
                if (weapons[i].image != null)
                {
                    slotImages[i].texture = weapons[i].image;
                }
                else
                {
                    slotImages[i].texture = Resources.Load<Texture>("NoImage.png");
                }
            }
            else
            {
                slotImages[i].texture = null;

            }
        }
    }

    public void SlotUpdate(Weapon weapons,int index)
    {
        if (weapons.image != null)
        {
            slotImages[index].texture = weapons.image;
        }
        else
        {
            slotImages[index].texture = Resources.Load<Texture>("NoImage.png");
        }
    }

    public void SetActiveSlot(int index)
    {
        for(int i = 0; i < slotPanels.Count; i++)
        {
            if (i == index)
            {
                slotPanels[i].color = new Color(0.7f,0.7f,0.7f);
            }
            else
            {
                slotPanels[i].color = new Color(0.3f,0.3f,0.3f);
            }
        }
    }

    public void Damage(float now,float max)
    {
        HPSlider.value = now / max;
        var fill =HPSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        if (fill.color !=damageColor)
            fill.color = damageColor;
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            fill.color = new Color((float)0x43 / 255f, (float)0xA0 / 255f, (float)0x47 /255f);
        }));
    }
    public void Health(float now, float max)
    {
        HPSlider.value = now / max;
        var fill = HPSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        if (fill.color != new Color(1, 0.3f, 0.3f))
            fill.color = new Color(0.3f, 0.6f, 0.6f);
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            fill.color = new Color((float)0x43 / 255f, (float)0xA0 / 255f, (float)0x47 / 255f);
        }));
    }

    public void setGrenade(float value)
    {
        GrenadeSlider.value = value;
        //var fill = GrenadeSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
    }

    public void setTargetCount(int v,int maxValue)
    {
        targetCountUI.textUpdate(v, maxValue);
    }
    public void setTargetCount(int v)
    {
        targetCountUI.textUpdate(v);
    }

    public void setTargetPointer(List<GameObject> targets)
    {
        targetCountUI.TargetPointerStart(targets,camera);
    }

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
