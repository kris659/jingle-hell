using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        retryButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(MainMenuButton);
    }

    public void OpenUI(string text)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        titleText.text = text;
        Time.timeScale = 0;
    }
    void PlayAgain()
    {
        MainMenu.skipPlay = true;
        SceneManager.LoadScene("KrisScene");
        //transform.GetChild(0).gameObject.SetActive(false);
        //UIManager.mainMenu.OpenUI();
        //UIManager.mainMenu.StartGame();
    }
    void MainMenuButton()
    {
        SceneManager.LoadScene("KrisScene");
        //transform.GetChild(0).gameObject.SetActive(false);
        //UIManager.mainMenu.OpenUI();
    }

}
