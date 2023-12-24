using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static ChargingUI chargingUI;
    [SerializeField] private ChargingUI _chargingUI;

    public static PlayerHealthUI playerHealthUI;
    [SerializeField] private PlayerHealthUI _playerHealthUI;

    public static SantaHealthUI santaHealthUI;
    [SerializeField] private SantaHealthUI _santaHealthUI;

    public static MainMenu mainMenu;
    [SerializeField] private MainMenu _mainMenu;

    public static GameOverUI gameOverUI;
    [SerializeField] private GameOverUI _gameOverUI;

    public static TutorialUI tutorialUI;
    [SerializeField] private TutorialUI _tutorialUI;


    private void Start()
    {
        chargingUI = _chargingUI;
        playerHealthUI = _playerHealthUI;
        santaHealthUI = _santaHealthUI;
        mainMenu = _mainMenu;
        gameOverUI = _gameOverUI;
        tutorialUI = _tutorialUI;
    }
}
