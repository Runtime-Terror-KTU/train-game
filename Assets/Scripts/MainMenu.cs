using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public SaveLoad saveLoad = new SaveLoad();
  
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("LoadState", 1);
        SceneManager.LoadScene("TempScene");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TempScene");
    }
}
