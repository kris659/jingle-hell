using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingUI : MonoBehaviour
{
    [SerializeField] private Image image;

    
    public void UpdateImage(float fillAmount)
    {
        Debug.Log(fillAmount);
        image.fillAmount = fillAmount;
    }
}
