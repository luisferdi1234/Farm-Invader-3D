using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class WinScreenCanvas : MonoBehaviour
{
    GameObject pause;
    GameObject ui;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreDescriptionText;


    public void OnLevelWon()
    {
        Time.timeScale = 0f;

        pause = GameObject.Find("Pause Canvas");
        pause.SetActive(false);
        ui = GameObject.Find("PlayerCanvas");
        ui.SetActive(false);

        scoreDescriptionText.text = $"Times Spotted By Farmers: {ScoreManager.Instance.spot}";
        scoreDescriptionText.text += $"\n Cows collected: {ScoreManager.Instance.cows} / {ScoreManager.Instance.maxAmountOfCows}";

        int score = ScoreManager.Instance.AddUpScore();

        if (score == 1)
        {
            scoreText.text = $"Level Score: \n Bronze";
        }
        else if (score == 2)
        {
            scoreText.text = $"Level Score: \n Silver";
        }
        else if (score == 3)
        {
            scoreText.text = $"Level Score: \n Gold";
        }

        //Updates Data
        string currentLevel = SceneManager.GetActiveScene().name;
        PlayerProgressManager.Instance.LevelComplete(currentLevel, score);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        PlayerProgressManager.Instance.menuMusic.Play();
        SceneManager.LoadScene("Main Menu");
    }

    public void LevelSelect()
    {
        Time.timeScale = 1f;

        // Load the next scene
        SceneManager.LoadScene("LevelSelect");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
