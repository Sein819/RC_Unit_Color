using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class setting : MonoBehaviour
{
    public GameObject pauseCanvas;

    void Start()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        AudioListener.pause = false;
    }

    public void Setting()
    {
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
        AudioListener.pause = true;
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        AudioListener.pause = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
