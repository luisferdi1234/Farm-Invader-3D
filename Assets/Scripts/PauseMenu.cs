using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] public GameObject pauseMenuUI;
    [SerializeField] private GameObject gadgetsMenuUI;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI obtainableCows;

    private GameObject ui;

    [SerializeField] GameObject restartImage;
    [SerializeField] Sprite R;
    [SerializeField] Sprite PSPause;
    [SerializeField] Sprite XboxPause;

    PlayerControls playerControls;
    InputAction pause;
    InputAction restart;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();
        ui = GameObject.Find("PlayerCanvas");
    }

    private void Start()
    {
        //Waits for score manager to be instantiated
        levelName.text = ScoreManager.Instance.levelName;

        //Turns off restart button on mobile
        if (Application.isMobilePlatform)
        {
            restartImage.SetActive(false);
        }

        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GetComponent<Canvas>().planeDistance = 1;
    }

    private void OnEnable()
    {
        //Input System
        pause = playerControls.Player.Pause;
        pause.Enable();
        pause.performed += Pause;

        restart = playerControls.Player.Restart;
        restart.Enable();
        restart.performed += RestartPressed;
    }

    private void OnDisable()
    {
        pause.Disable();

        restart.Disable();
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

    /// <summary>
    /// Handles the restart button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void RestartPressed(InputAction.CallbackContext context)
    {
        Restart();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume()
    {
        //Turns off pause menu
        pauseMenuUI.SetActive(false);

        //Turns off gadget screen if it's on
        gadgetsMenuUI.SetActive(false);

        //Turns on player UI
        ui.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        if (ScoreManager.Instance.cows == 0)
        {
            obtainableCows.color = Color.red;
        }
        else if (ScoreManager.Instance.cows < ScoreManager.Instance.maxAmountOfCows)
        {
            obtainableCows.color = Color.yellow;
        }
        else
        {
            obtainableCows.color = Color.green;
        }

        //Turns off player UI
        ui.SetActive(false);

        //Makes it so it only detects controllers on pc
        if (!Application.isMobilePlatform)
        {
            //Updates restart image
            CheckConnectedControllers();
        }

        //Shows how many cows the player has collected so far
        obtainableCows.text = $"Cows: {ScoreManager.Instance.cows}/{ScoreManager.Instance.maxAmountOfCows}";


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

    void CheckConnectedControllers()
    {
        bool keyboardUsed = InputSystem.GetDevice<Keyboard>().anyKey.isPressed;
        bool mouseUsed = InputSystem.GetDevice<Mouse>().leftButton.isPressed || InputSystem.GetDevice<Mouse>().rightButton.isPressed;

        Image restart = restartImage.GetComponent<Image>();

        if (keyboardUsed || mouseUsed)
        {
            DisplayPrompt(restart, R);
        }
        else
        {
            foreach (var device in InputSystem.devices)
            {
                Debug.Log("Device: " + device.name);
                if (device is Gamepad gamepad)
                {
                    // Check for specific controllers based on name
                    if (gamepad.name.Contains("Xbox")) // Xbox controller
                    {
                        DisplayPrompt(restart, XboxPause);
                    }
                    else if (gamepad.name.Contains("Sony") || gamepad.name.Contains("DualSense") || gamepad.name.Contains("DualShock")) // PS4 or PS5 controller
                    {
                        DisplayPrompt(restart, PSPause);
                    }
                }
            }
        }
    }

    void DisplayPrompt(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }
}
