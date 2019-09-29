using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : RocketScript
{
    [SerializeField] public TriggerListenner area;

    [SerializeField] public bool adsorption=false;

    [SerializeField] public float explodeAfter = 0.5f;

    public enum CollideType
    {
        nomal,
        adsorption,
        flash
    }

    [SerializeField] public CollideType collidType;

    public enum StrongAgainst
    {
        nomal,
        Enemy,
        Building
    }

    [SerializeField] public StrongAgainst strong;

    bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        area.action = OnTriggerEnater_;
        rb=GetComponent<Rigidbody>();
        area.gameObject.SetActive(false);
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
        if (flyTime >= TimeLimit&& flyTime < TimeLimit + explodeDelay)
        {
            if (!exploded)
            {
                explode();
                //Destroy(gameObject);
            }
            else
            {
                area.transform.localScale = new Vector3(1, 1, 1) * (float)Mathf.Pow((flyTime - TimeLimit) / (explodeDelay), 2) * 100 * ((collidType == CollideType.flash) ? 0.5f : 1f);
                //area.transform.Rotate(new Vector3(0, 90*Time.deltaTime*Random.value*10, 0));
            }
        }
        if (flyTime >= TimeLimit + explodeDelay + explodeAfter)
        {
            Destroy(gameObject);
        }
        //Debug.Log(Mathf.Pow((flyTime - TimeLimit) / (explodeDelay), 5f));

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collidType==CollideType.adsorption)
        {
            var jrb = collision.gameObject.GetComponent<Rigidbody>();
            var joint = gameObject.AddComponent<FixedJoint>();
            if (jrb == null)
            {
                joint.connectedAnchor = collision.transform.position;
            }
            else
            {
                
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
        var character = otherObj.GetComponent<Character>();
        if (character != null)
        {
            var damage = explodeDamageValue;
            if(collidType == CollideType.flash)
            {
                damage *= 0.7f;
            }

            if(character is EnemyController&&strong==StrongAgainst.Enemy)
            {
                damage *= 2.5f;
            }
            else if((character is Building || character.gameObject.layer == 10) && strong == StrongAgainst.Building)
            {
                damage *= 100;
            }
            else
            {
                damage *= 1.5f;
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
