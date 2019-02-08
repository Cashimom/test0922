using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour {

    [SerializeField] private GameObject WarpTarget;
    [SerializeField] private float nextWarpDelay=5;
    [SerializeField] private float warpHigh = 20;
    private float count = 0.0f;
    private bool cameON = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cameON)
        {
            count += Time.deltaTime;
            if (count > nextWarpDelay)
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
        position.y += warpHigh;
        gameObject.transform.position = position;
        cameON = true;
    }
}
