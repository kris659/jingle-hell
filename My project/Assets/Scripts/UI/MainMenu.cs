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
        transform.GetChild(0).gameObject.SetActive(true);
        centerPoint.SetActive(false);
    }

    private void StartGame()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        personController.enabled = true;
        centerPoint.SetActive(true);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

}
