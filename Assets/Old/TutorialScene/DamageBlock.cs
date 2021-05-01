using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    [SerializeField] public float damage = 10;

    [SerializeField] public float force = 10;

    [SerializeField] public Vector3 forceVec = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var charcter = collision.gameObject.GetComponent<Character>();
        if (charcter != null)
        {
            charcter.explodeDamage(damage);
            charcter.GetComponent<Rigidbody>().AddForce(
                (forceVec)*force,
                ForceMode.Impulse
                );
        }
    }
}
