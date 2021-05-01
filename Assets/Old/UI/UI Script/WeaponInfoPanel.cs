using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoPanel : MonoBehaviour
{
    [SerializeField] private RawImage weaponImage;

    [SerializeField] private ListChild listChild_Prefab;

    [SerializeField] private GameObject scrollviewContent;

    private Weapon _weapon;
    public Weapon weapon
    {
        get
        {
            return _weapon;
        }
        set
        {
            _weapon = value;
            createInfoList();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createInfoList()
    {
        for (int i = 0; i < scrollviewContent.transform.childCount; i++)
        {
            Destroy(scrollviewContent.transform.GetChild(i).gameObject);
        }

        if (_weapon == null)
        {
            weaponImage.texture = null;
            return;
        }

        weaponImage.texture = _weapon.image;
        foreach (var i in _weapon.weaponInfoList)
        {
            var child = Instantiate(listChild_Prefab.gameObject);
            child.GetComponent<ListChild>().setState(i.Key, i.Value);
            child.transform.parent = scrollviewContent.transform;
        }
    }

}
