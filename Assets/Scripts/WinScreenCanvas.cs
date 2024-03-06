using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenCanvas : MonoBehaviour
{
    [SerializeField] GameObject pause;
    [SerializeField] GameObject ui;
    
    
    public void OnLevelWon()
    {
        Time.timeScale = 0f;
        pause.SetActive(false);
        ui.SetActive(false);
        string currentLevel = SceneManager.GetActiveScene().name;
        PlayerProgressManager.Instance.LevelComplete(currentLevel);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the index of the next scene
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }
}
