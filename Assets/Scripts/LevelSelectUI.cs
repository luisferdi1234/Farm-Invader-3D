using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] GameObject UFO;
    UFOController ufoController;

    private void Awake()
    {
        ufoController = UFO.GetComponent<UFOController>();
    }

    private void Update()
    {
        if (ufoController.levelObject != null)
        {
            LevelInfo levelInfo = ufoController.levelObject.GetComponent<LevelInfo>();
            background.enabled = true;
            if(levelInfo.score == -1)
            {
                ShowMenuText();
            }
            else
            {
                ShowLevelText(levelInfo);
            }
        }
        else
        {
            background.enabled = false;
            levelNameText.text = "";
            currentScore.text = "";
        }
    }

    private void ShowMenuText()
    {
        LevelInfo levelInfo = ufoController.levelObject.GetComponent<LevelInfo>();
        background.enabled = true;
        levelNameText.text = $"{levelInfo.gameObject.name}: \n {levelInfo.levelDescription}";
        currentScore.text = "";
    }

    private void ShowLevelText(LevelInfo levelInfo)
    {
        levelNameText.text = $"Level {ufoController.currentLevel[ufoController.currentLevel.Length - 1].ToString()}: \n{levelInfo.levelDescription}";
        if (levelInfo.score == 1)
        {
            currentScore.text = $"Current Score: \nBronze";
        }
        else if (levelInfo.score == 2)
        {
            currentScore.text = $"Current Score: \nSilver";
        }
        else if (levelInfo.score == 3)
        {
            currentScore.text = $"Current Score: \nGold";
        }
    }
}
