using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image image;
    private bool isUIHidden;

    private void Awake()
    {
        HideUI();
    }

    public void UpdateUI(float fillAmount)
    {
        if(fillAmount < 0)
            fillAmount = 0;
        if(fillAmount > 1)
            fillAmount = 1;
        if (isUIHidden)
            ShowUI();
        image.fillAmount = fillAmount;
    }

    private void ShowUI()
    {
        isUIHidden = false;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideUI()
    {
        isUIHidden = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
