using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<RawImage> slotImages;
    [SerializeField] private List<Image> slotPanels;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private Slider GrenadeSlider;

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

    public void SlotUpdate(List<Weapon> weapons)
    {
        for(int i = 0; i < weapons.Count; i++)
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
        if (fill.color != new Color(1, 0.3f, 0.3f))
            fill.color = new Color(1, 0.3f, 0.3f);
        StartCoroutine(DelayMethod(0.1f, () =>
        {
            fill.color = new Color((float)0x17 / 255f, (float)0xB4 / 255f, 0);
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
            fill.color = new Color((float)0x17 / 255f, (float)0xB4 / 255f, 0);
        }));
    }

    public void setGrenade(float value)
    {
        GrenadeSlider.value = value;
        //var fill = GrenadeSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
    }

    public IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
