using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector]
    public float weight;

    public Vector3 holdingOffset;

    [HideInInspector]
    public Vector3 normalScale;
    public Vector3 holdingScale;

    private void Start()
    {
        normalScale = transform.localScale;
        weight = GetComponent<Rigidbody>().mass;
    }
}
