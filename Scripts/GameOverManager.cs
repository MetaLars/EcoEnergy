using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public Button MainMenuButton;
    public Button RestartButton;
    public Button QuitButton;

    void Start()
    {
        gameOverPanel.SetActive(false);

        MainMenuButton.onClick.AddListener(GGoToMainMenu);
        RestartButton.onClick.AddListener(RRestart);
        QuitButton.onClick.AddListener(QQuitGame);
    }

    private void Update()
    {
        if (IsCriticalStatLow())
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    bool IsCriticalStatLow()
    {
        return StatManager.Instance.totalEnergy <= -25 ||
               StatManager.Instance.totalWater <= -25 ||
               StatManager.Instance.totalNature <= -25 ||
               StatManager.Instance.totalSecurity <= -25 ||
               StatManager.Instance.totalHappiness <= -25;
    }

    void GGoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RRestart()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void QQuitGame()
    {
        Application.Quit();
    }  
}
