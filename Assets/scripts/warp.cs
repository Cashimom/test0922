using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour {

    public GameObject WarpTarget;
    float count = 0.0f;
    bool cameON = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cameON)
        {
            count += Time.deltaTime;
            if (count > 5)
            {
                count = 0.0f;
                cameON = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!cameON&&other.gameObject.tag=="Character")
        {
            WarpTarget.GetComponent<warp>().warpHere(other.gameObject);
        }
    }

    void warpHere(GameObject gameObject)
    {
        Vector3 position = transform.position;
        position.y += 20;
        gameObject.transform.position = position;
        cameON = true;
    }
}
