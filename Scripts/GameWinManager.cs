using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    public GameObject winPanel;
    public int requiredPopulationToWin = 10000;
    public int requiredDaysToWin = 30;

    private bool hasWon = false;

    void Start()
    {
        winPanel.SetActive(false);
    }

    void Update()
    {
        if (hasWon) return;

        bool populationReached = StatManager.Instance.totalPopulation >= requiredPopulationToWin;
        bool survived30Days = Sun.Instance.currentDay >= requiredDaysToWin;

        if (populationReached || survived30Days)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        hasWon = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
