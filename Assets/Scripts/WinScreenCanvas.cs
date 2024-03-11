using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class WinScreenCanvas : MonoBehaviour
{
    [SerializeField] GameObject pause;
    [SerializeField] GameObject ui;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreDescriptionText;


    public void OnLevelWon()
    {
        Time.timeScale = 0f;
        pause.SetActive(false);
        ui.SetActive(false);
        string currentLevel = SceneManager.GetActiveScene().name;

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

        PlayerProgressManager.Instance.LevelComplete(currentLevel, score);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        PlayerProgressManager.Instance.menuMusic.Play();
        SceneManager.LoadScene("Main Menu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        // Load the next scene
        SceneManager.LoadScene("LevelSelect");
    }
}
