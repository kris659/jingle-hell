using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static ChargingUI chargingUI;
    [SerializeField] private ChargingUI _chargingUI;

    public static PlayerHealthUI playerHealthUI;
    [SerializeField] private PlayerHealthUI _playerHealthUI;

    public static SantaHealthUI santaHealthUI;
    [SerializeField] private SantaHealthUI _santaHealthUI;


    private void Start()
    {
        chargingUI = _chargingUI;
        playerHealthUI = _playerHealthUI;
        santaHealthUI = _santaHealthUI;
    }
}
