using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] public string sceneName;
    [SerializeField] public string previousLevel;
    [SerializeField] public string levelName;
    [SerializeField] public string levelDescription;
    [SerializeField] public string attainableCows;
    public int score;

    private void Start()
    {
        if (sceneName == "Level1" || PlayerPrefs.HasKey(previousLevel))
        {
            score = PlayerPrefs.GetInt(sceneName);
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
        else if (score != -1)
        {
            gameObject.SetActive(false);
        }
    }
}
