using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button quitrButton;

    void Start()
    {
        playButton.onClick.AddListener(Play);
        quitrButton.onClick.AddListener(quit);
    }
    public void Play()
    {
        SceneManager.LoadScene("Oyun");
    }

    public void quit()
    {
        Application.Quit();
    }
}
