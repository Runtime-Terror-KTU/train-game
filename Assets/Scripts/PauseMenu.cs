using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool OnPause = false;

    public GameObject PauseMen;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OnPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMen.SetActive(false);
        Time.timeScale = 1f;
        OnPause = false;
    }

    void Pause()
    {
        PauseMen.SetActive(true);
        Time.timeScale = 0f;
        OnPause = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
