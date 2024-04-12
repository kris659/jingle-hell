using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;

    public void UpdateText(string text)
    {
        if (text == string.Empty)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);
        tutorialText.text = text;
    }
}
