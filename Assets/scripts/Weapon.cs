using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isHave = false;
    public GameObject rocket;
    public Transform ShotTransform;
    protected bool near = false;
    public Character character;
    public TextMeshProUGUI pressButton;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
        if (near&&(!isHave))
        {
            if (Input.GetKeyDown("f"))
            {
                character.Weapon.DropWeapon();
                HaveWeapon();
                near = false;
                pressButton.enabled = true;
                character.Weapon = this;
            }
        }
	}

    public RocketScript Fire()
    {
        var fire = Instantiate(rocket, ShotTransform.position + ShotTransform.forward * 2, ShotTransform.rotation);
        return fire.GetComponent<RocketScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            near = true;
            character = other.gameObject.GetComponent<Character>();
            pressButton.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            near = false;
            //character = null;
            pressButton.enabled = false;
        }
    }

    public void DropWeapon()
    {
        isHave = false;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void HaveWeapon()
    {
        isHave = true;
        GetComponent<BoxCollider>().enabled = false;
    }
}
