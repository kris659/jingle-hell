using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnterTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            UIManager.tutorialUI.UpdateText("Pick up an anvil");
            Destroy(gameObject);
        }
    }
}
