using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDeletionManager : MonoBehaviour
{
    public static DataDeletionManager instance;
    GameObject[] levels; 
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        levels = GameObject.FindGameObjectsWithTag("Level");
    }

    /// <summary>
    /// Sets all other levels in not active
    /// </summary>
    public void DeleteAllOtherLevels()
    {
        foreach (GameObject level in levels)
        {
            LevelInfo levelInfo = level.GetComponent<LevelInfo>();

            if (!(levelInfo.score == -1 || levelInfo.levelName == "Level1"))
            {
                if (level.active)
                {
                    level.SetActive(false);
                }
            }
        }
    }
}
