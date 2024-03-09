using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    GameObject canvas;
    [SerializeField] public int maxAmountOfCows;

    public int spot = 0;
    public int cows = 0;
    public int completion = 0;

    private void Awake()
    {
        Instance = this;
        canvas = GameObject.Find("WinScreenCanvas");
        canvas.SetActive(false);
    }

    /// <summary>
    /// Adds one to spot tally
    /// </summary>
    public void SpottedAlien()
    {
        spot += 1;
    }

    /// <summary>
    /// Adds one to cow tally
    /// </summary>
    public void AddUpCows()
    {
        cows += 1;
        if (cows >= maxAmountOfCows)
        {
            LevelCompleted();
        }
    }

    /// <summary>
    /// Checks if a cow has been abducted and turns on win screen
    /// </summary>
    public bool LevelCompleted()
    {
        if (cows >= 1)
        {
            completion += 1;
            canvas.SetActive(true);
            canvas.GetComponent<WinScreenCanvas>().OnLevelWon();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int AddUpScore()
    {
        if (spot >= 1)
        {
            spot = 0;
        }
        else if (spot == 0)
        {
            spot = 1;
        }
        if (cows >= maxAmountOfCows)
        {
            cows = 1;
        }
        else
        {
            cows = 0;
        }
        return spot + completion + cows;
    }
}
