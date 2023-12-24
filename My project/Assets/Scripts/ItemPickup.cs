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
    private GameObject heldItem;
    public static bool tutorialPickup = false;

    private GameObject throwingSound;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (isHoldingItem){
                isChargingThrow = true;
                chargingStartTime = Time.time;
                throwingSound = AudioManager.PlaySound(AudioManager.Sound.AnvilThrowCharge);
            }
            else 
                TryToPickUp();
        if (isChargingThrow && Input.GetMouseButtonUp(0))
            ThrowItem();
        if (isChargingThrow)
            UIManager.chargingUI.UpdateUI((Time.time - chargingStartTime) / maxChargingTime);
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
                if (!tutorialPickup){
                    UIManager.tutorialUI.UpdateText("Hit an elf with an anvil");
                    tutorialPickup = true;
                }

                AudioManager.PlaySound(AudioManager.Sound.AnvilPickup);

                isHoldingItem = true;
                heldItem = hit.transform.gameObject;
                //heldItem.transform.localEulerAngles = Vector3.zero;
                heldItem.transform.parent = playerCamera.transform;
                heldItem.transform.localPosition = itemScript.holdingOffset;
                heldItem.transform.localScale = itemScript.holdingScale;
                heldItem.GetComponent<Collider>().enabled = false;
                itemScript.onPickup?.Invoke(itemScript);
                itemScript.onPickup = null;
                if(itemScript.TryGetComponent(out Rigidbody rb))
                    Destroy(rb);
            }
        }
    }

    private void ThrowItem()
    {
        Destroy(throwingSound);
        AudioManager.PlaySound(AudioManager.Sound.AnvilThrow);
        UIManager.chargingUI.HideUI();

        isChargingThrow = false;
        isHoldingItem = false;
        Item itemScript = heldItem.GetComponent<Item>();
        heldItem.transform.parent = null;
        heldItem.transform.position += playerCamera.transform.forward * 0.2f;
        heldItem.transform.localScale = itemScript.normalScale;
        heldItem.GetComponent<Collider>().enabled = true;
        Rigidbody rb = heldItem.AddComponent<Rigidbody>();
        rb.mass = itemScript.weight;
        Debug.Log("Mass: " + rb.mass);

        rb.AddForce(GetThrowForce(), ForceMode.Impulse);
    }

    private Vector3 GetThrowForce()
    {
        Debug.Log(Time.time - chargingStartTime + "  " + throwingForceByTime.Evaluate(Mathf.Clamp01((Time.time - chargingStartTime) / maxChargingTime)));
        Vector3 force = playerCamera.transform.forward * throwingForce * throwingForceByTime.Evaluate(Mathf.Clamp01((Time.time - chargingStartTime) / maxChargingTime));
        return force;
    }
}
