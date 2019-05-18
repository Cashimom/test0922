using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] Camera camera;

    [SerializeField] Transform head;

    //[SerializeField] playerController player;

    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = head.transform.position;
        //Vector3 pos = player.transform.position;
        pos += (new Vector3(0, -1, 0)) + head.transform.forward * 100;
        Vector3 pos_s= RectTransformUtility.WorldToScreenPoint(camera, pos);
        pos_s.y -= 10;
        if ((pos_s - rect.position).magnitude < 100)
            rect.position = Vector3.Lerp(rect.position, pos_s, 0.05f);
    }
}
