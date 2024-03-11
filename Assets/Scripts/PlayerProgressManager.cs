using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    // Singleton instance
    public static PlayerProgressManager Instance;

    public Vector3 playerPosition = Vector3.zero;

    public AudioSource menuMusic;

    // Optional: Other initialization logic can go here

    private void Awake()
    {
        // Ensure there's only one instance
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        menuMusic = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (menuMusic == null)
        {
            menuMusic = GetComponent<AudioSource>();
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

    /// <summary>
    /// Saves the position of the UFO
    /// </summary>
    public void SaveLevelSelectPosition(GameObject ufo)
    {
        playerPosition = ufo.transform.position;
    }
}
