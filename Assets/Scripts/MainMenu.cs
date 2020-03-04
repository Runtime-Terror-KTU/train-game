using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        //do something
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TempScene");
    }
}
