using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] public GameObject pauseMenuUI;

    PlayerControls playerControls;
    InputAction pause;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        //Input System
        pause = playerControls.Player.Pause;
        pause.Enable();
        pause.performed += Pause;
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    /// <summary>
    /// Handles the invisble button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void Pause(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        PlayerProgressManager.Instance.menuMusic.Play();
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
