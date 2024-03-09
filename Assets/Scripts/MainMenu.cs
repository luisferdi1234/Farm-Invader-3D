using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI movementText;
    [SerializeField] private TMPro.TextMeshProUGUI pickUpText;
    [SerializeField] private TMPro.TextMeshProUGUI gadgetText;
    [SerializeField] private TMPro.TextMeshProUGUI rechargeText;

    private void Update()
    {
        // Check for connected game controllers
        CheckConnectedControllers();
    }

    void CheckConnectedControllers()
    {
        bool keyboardUsed = InputSystem.GetDevice<Keyboard>().anyKey.isPressed;
        bool mouseUsed = InputSystem.GetDevice<Mouse>().leftButton.isPressed || InputSystem.GetDevice<Mouse>().rightButton.isPressed;

        if (keyboardUsed || mouseUsed)
        {
            DisplayPrompt("Use WASD to move", movementText);
            DisplayPrompt("Use 'Left Click' to pick up items and cows", pickUpText);
            DisplayPrompt("Use 'Right Click' to use gadgets", gadgetText);
            DisplayPrompt("Use 'Space' to recharge gadget while next to a cow and to extract from level (while near a UFO)", rechargeText);
        }
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad gamepad)
            {
                // Check for specific controllers based on name
                if (gamepad.name.Contains("Xbox")) // Xbox controller
                {
                    DisplayPrompt("Use Left Stick to move", movementText);
                    DisplayPrompt("Use 'A' to pick up items and cows", pickUpText);
                    DisplayPrompt("Use 'X' to use gadgets", gadgetText);
                    DisplayPrompt("Use 'B' to recharge gadgets while next to a cow and to extract from level (while near a UFO)", rechargeText);
                }
                else if (gamepad.name.Contains("Sony")) // PS4 or PS5 controller
                {
                    DisplayPrompt("Use Left Stick to move", movementText);
                    DisplayPrompt("Use 'X' to pick up items and cows", pickUpText);
                    DisplayPrompt("Use 'Square' to use gadgets", gadgetText);
                    DisplayPrompt("Use 'Circle' to recharge gadgets while next to a cow and to extract from level (while near a UFO)", rechargeText);
                }
            }
        }
    }

    void DisplayPrompt(string message, TMPro.TextMeshProUGUI TMPtext)
    {
        TMPtext.text = message;
    }

    public void Play()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
