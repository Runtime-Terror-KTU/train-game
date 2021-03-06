﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool OnPause = false;

    public GameObject PauseMen;
    public GameObject crosshair;
    public SaveLoad pauseManager;
    public GameObject explanation;
    public GameObject gameEnd;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OnPause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                crosshair.SetActive(true);
                explanation.SetActive(true);
                gameEnd.SetActive(true);
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                crosshair.SetActive(false);
                explanation.SetActive(false);
                gameEnd.SetActive(false);
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

    public void Save()
    {
        pauseManager.isSaving = true;
    }

    public void Load()
    {
        Resume();
        pauseManager.isLoading = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
