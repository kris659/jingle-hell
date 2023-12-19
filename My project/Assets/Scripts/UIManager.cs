using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static ChargingUI chargingUI;
    [SerializeField] private ChargingUI _chargingUI;


    private void Start()
    {
        chargingUI = _chargingUI;
    }
}
