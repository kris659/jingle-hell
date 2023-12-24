using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;

    public Button exitButton;

    private void Awake()
    {
        masterVolume.value = AudioManager.masterVolume;
        musicVolume.value = AudioManager.musicVolume;
        sfxVolume.value = AudioManager.sfxVolume;

        masterVolume.onValueChanged.AddListener(MasterValueChanged);
        musicVolume.onValueChanged.AddListener(MusicValueChanged);
        sfxVolume.onValueChanged.AddListener(SfxValueChanged);

        exitButton.onClick.AddListener(CloseSettings);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            CloseSettings();
    }

    public void MasterValueChanged(float value)
    {
        AudioManager.masterVolume = value;
    }
    public void MusicValueChanged(float value)
    {
        AudioManager.musicVolume = value;
    }
    public void SfxValueChanged(float value)
    {
        AudioManager.sfxVolume = value;
    }

    public void CloseSettings(){
        PlayerPrefs.SetFloat("MasterVolume", AudioManager.masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", AudioManager.musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", AudioManager.sfxVolume);
        //AudioManager.PlaySound(AudioManager.Sound.ButtonUI);
        if(UIManager.mainMenu.transform.GetChild(0).gameObject.activeSelf == false)
            Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
       
    }


    private void OnEnable()
    {
        Time.timeScale = 0.01f;
        Cursor.lockState = CursorLockMode.None;
    }
}
