using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] public string levelName;
    [SerializeField] public string previousLevel;
    [SerializeField] public string levelDescription;
    public int score;

    private void Start()
    {
        if (levelName == "Level1" || PlayerPrefs.HasKey(previousLevel))
        {
            score = PlayerPrefs.GetInt(levelName);
            if (score == 1)
            {
                GetComponent<Outline>().OutlineColor = Color.red;
            }
            else if (score == 2)
            {
                GetComponent<Outline>().OutlineColor = Color.gray;
            }
            else if (score == 3)
            {
                GetComponent<Outline>().OutlineColor = Color.yellow;
            }
        }
        else if (levelName != "Main Menu")
        {
            gameObject.SetActive(false);
        }
    }
}
