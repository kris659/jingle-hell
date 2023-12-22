using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector]
    public float weight;

    public float damageMult;

    public Vector3 holdingOffset;

    [HideInInspector]
    public Vector3 normalScale;
    public Vector3 holdingScale;

    public delegate void OnPickup(Item item);
    public OnPickup onPickup;

    private void Start()
    {
        onPickup += Init;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out ITakingDamage target)){            
            target.TakeDamage(collision.impulse.magnitude * damageMult);
        }
    }
    
    void Init(Item item)
    {
        if(TryGetComponent(out Rigidbody rb)){
            Destroy(rb);
        }
        normalScale = transform.localScale;
        //weight = GetComponent<Rigidbody>().mass;
    }
}
