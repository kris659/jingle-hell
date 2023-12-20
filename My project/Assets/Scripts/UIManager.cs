using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static ChargingUI chargingUI;
    [SerializeField] private ChargingUI _chargingUI;

    public static HealthUI healthUI;
    [SerializeField] private HealthUI _healthUI;


    private void Start()
    {
        chargingUI = _chargingUI;
        healthUI = _healthUI;
    }
}
