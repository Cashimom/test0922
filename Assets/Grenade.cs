using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : RocketScript
{
    [SerializeField] public TriggerListenner area;

    [SerializeField] public bool adsorption=false;

    public enum CollideType
    {
        nomal,
        adsorption,
        flash
    }

    [SerializeField] public CollideType collidType;

    bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        area.action = OnTriggerEnater_;
        rb=GetComponent<Rigidbody>();
        //GetComponent<FixedJoint>().
    }

    // Update is called once per frame
    void Update()
    {
        flyTime += Time.deltaTime;
        if (!exploded)
        {
            rb.AddForce(new Vector3(0, -1000, 0));
        }
        if (flyTime >= TimeLimit&&!exploded)
        {
            explode();
            //Destroy(gameObject);
        }
        if (exploded)
        {
            area.transform.localScale = new Vector3(1,1,1)*(flyTime-TimeLimit)/(explodeDelay)*100*((collidType == CollideType.flash)?0.5f:1f);
            area.transform.Rotate(new Vector3(0, 90*Time.deltaTime*Random.value*10, 0));
        }

        if (flyTime >= TimeLimit + explodeDelay)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collidType==CollideType.adsorption)
        {
            var jrb = collision.gameObject.GetComponent<Rigidbody>();
            if (jrb == null)
            {
                //joint.connectedAnchor = collision.transform.position;
            }
            else
            {
                var joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = jrb;
            }
        }
        else if(collidType == CollideType.flash&&!exploded)
        {
            flyTime = TimeLimit;
            explode();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        var otherObj = other.gameObject;
        if (otherObj.GetComponent<Character>() != null)
        {
            var damage = explodeDamageValue;
            if(collidType == CollideType.flash)
            {
                damage *= 0.7f;
            }
            otherObj.GetComponent<Character>().explodeDamage(damage, parent);
        }
    }

    void explode()
    {
        area.gameObject.SetActive(true);
        
        exploded = true;
        rb.Sleep();
    }

    private void OnTriggerEnater_(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            //gameObject.SetActive(false);
        }
    }
    
}
