using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private float pickupRange;
    [SerializeField] private LayerMask pickupLayer;

    [SerializeField] private float throwingForce;
    [SerializeField] private AnimationCurve throwingForceByTime;
    [SerializeField] private float maxChargingTime;


    private bool isHoldingItem = false;
    private bool isChargingThrow = false;
    private float chargingStartTime = 0;
    private GameObject holdedItem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (isHoldingItem){
                isChargingThrow = true;
                chargingStartTime = Time.time;
            }
            else 
                TryToPickUp();
        if (isChargingThrow && Input.GetMouseButtonUp(0))
            ThrowItem();
    }

    private void TryToPickUp()
    {
        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, pickupRange, pickupLayer))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.TryGetComponent(out Item itemScript)){
                isHoldingItem = true;
                holdedItem = hit.transform.gameObject;
                holdedItem.transform.parent = playerCamera.transform;
                holdedItem.transform.localPosition = itemScript.holdingOffset;
                holdedItem.transform.localScale = itemScript.holdingScale;
                holdedItem.GetComponent<Collider>().enabled = false;
                Destroy(holdedItem.GetComponent<Rigidbody>());                
            }
        }
    }

    private void ThrowItem()
    {
        isChargingThrow = false;
        isHoldingItem = false;
        Item itemScript = holdedItem.GetComponent<Item>();
        holdedItem.transform.parent = null;
        holdedItem.transform.position += playerCamera.transform.forward * 0.2f;
        holdedItem.transform.localScale = itemScript.normalScale;
        holdedItem.GetComponent<Collider>().enabled = true;
        Rigidbody rb = holdedItem.AddComponent<Rigidbody>();
        rb.mass = itemScript.weight;

        rb.AddForce(GetThrowForce(), ForceMode.Impulse);
    }

    private Vector3 GetThrowForce()
    {
        Debug.Log(Time.time - chargingStartTime + "  " + throwingForceByTime.Evaluate(Mathf.Clamp01((Time.time - chargingStartTime) / maxChargingTime)));
        Vector3 force = playerCamera.transform.forward * throwingForce * throwingForceByTime.Evaluate(Mathf.Clamp01((Time.time - chargingStartTime) / maxChargingTime));
        return force;
    }
}
