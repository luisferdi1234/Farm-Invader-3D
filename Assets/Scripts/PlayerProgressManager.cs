using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    // Singleton instance
    private static PlayerProgressManager _instance;

    // Public property to access the singleton instance
    public static PlayerProgressManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerProgressManager>();

                if (_instance == null)
                {
                    // If no instance found in the scene, create a new GameObject and add the script
                    GameObject singletonObject = new GameObject(typeof(PlayerProgressManager).Name);
                    _instance = singletonObject.AddComponent<PlayerProgressManager>();
                }
            }

            return _instance;
        }
    }

    // Optional: Other initialization logic can go here

    private void Awake()
    {
        // Ensure there's only one instance
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Optional: Other methods for player progress management

    public void LevelComplete(string levelName, int score)
    {
        //if the current score is higher, set new high score
        if (score > PlayerPrefs.GetInt(levelName))
        {
            // Save the progress using PlayerPrefs
            PlayerPrefs.SetInt(levelName, score);
            PlayerPrefs.Save();
            Debug.Log(PlayerPrefs.GetInt(levelName));
        }
    }
}
