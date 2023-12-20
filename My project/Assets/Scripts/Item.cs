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

    private void Start()
    {
        normalScale = transform.localScale;
        weight = GetComponent<Rigidbody>().mass;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out ITakingDamage target)){            
            target.TakeDamage(collision.impulse.magnitude * damageMult);
        }
    }
}
