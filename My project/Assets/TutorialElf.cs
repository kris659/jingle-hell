using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElf : MonoBehaviour
{
    public static GameObject door;
    private void OnDestroy()
    {
        UIManager.tutorialUI.UpdateText("Enter the next room");
        door.SetActive(false);
    }
}
