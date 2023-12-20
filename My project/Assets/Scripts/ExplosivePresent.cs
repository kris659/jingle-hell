using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePresent : MonoBehaviour
{
    [SerializeField] private float explosionPower;
    [SerializeField] private float explosionDamageMult;
    [SerializeField] private float explosionRadius;

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        Debug.Log(colliders.Length);
        for(int i = 0; i < colliders.Length; i++){
            if (colliders[i].gameObject.TryGetComponent(out PlayerHealth playerHealth))
                playerHealth.TakeDamage(explosionDamageMult * (explosionRadius - Vector3.Distance(transform.position, playerHealth.transform.position)));
        }
        for (int i = 0; i < colliders.Length; i++){
            if (colliders[i].gameObject.TryGetComponent(out Rigidbody rb)){
                Vector3 forceDirection = rb.transform.position - transform.position;
                float power = explosionPower * (explosionRadius - Vector3.Distance(transform.position, rb.transform.position));
                if (rb.GetComponent<PlayerHealth>() != null)
                    forceDirection.x *= 5;
                rb.AddForce(forceDirection  * power, ForceMode.Impulse);
            }
                
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
