using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject centerPoint;

    [SerializeField] private FirstPersonController personController;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject tutorialElfPrefab;
    [SerializeField] private Transform tutorialElfSpawnPoint;

    public static bool skipPlay = false;

    private void Awake()
    {
        OpenUI();
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        
    }

    public void OpenUI()
    {
        personController.enabled = false;
        personController.transform.position = spawnPoint.position;
        transform.GetChild(0).gameObject.SetActive(true);
        centerPoint.SetActive(false);
        Time.timeScale = 1f;
        if (skipPlay)
            StartGame();
        skipPlay = false;
    }

    public void StartGame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        personController.enabled = true;
        centerPoint.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        ItemPickup.tutorialPickup = false;
        GameObject elf = Instantiate(tutorialElfPrefab);
        elf.transform.position = tutorialElfSpawnPoint.transform.position;
    }

    private void ExitGame()
    {
        Application.Quit();
    }

}
