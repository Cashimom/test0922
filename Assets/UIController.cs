using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<RawImage> slotImages;
    [SerializeField] private List<Image> slotPanels;

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

}
