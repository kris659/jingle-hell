using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnter : MonoBehaviour
{
    public delegate void BossEnterDelegate();
    public static BossEnterDelegate OnBossEnter;

    public GameObject door;

    private void Awake()
    {
        TutorialElf.door = door;
    }
    private void Start()
    {
        OnBossEnter += () => { 
            door.SetActive(true);     
            UIManager.tutorialUI.UpdateText(string.Empty);
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            Debug.Log("PlayerEnteredBoss");
            OnBossEnter?.Invoke();
            OnBossEnter = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            Destroy(gameObject);
        }
    }

}
