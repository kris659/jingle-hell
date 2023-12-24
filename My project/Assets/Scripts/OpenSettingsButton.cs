using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSettingsButton : MonoBehaviour
{
    public Button button;
    public GameObject settings;

    private void Awake()
    {
        button.onClick.AddListener(() => { settings.SetActive(true);
            //AudioManager.PlaySound(AudioManager.Sound.ButtonUI);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if(settings.activeSelf)
                settings.GetComponent<SettingsUI>().CloseSettings();
            else 
                settings.SetActive(true);
    }
}
