using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
