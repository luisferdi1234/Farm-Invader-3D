using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownBarController : MonoBehaviour
{
    [SerializeField] GameObject childText;
    TextMeshProUGUI cooldownText;

    public float invisibilityCooldown = 0f; // Set the cooldown time in seconds
    private float maxCooldownTime = 15f;
    private Image cooldownBar;
    private GameObject player;
    private PlayerController playerController;

    private Color startColor = Color.red;
    private Color endColor = Color.blue;

    void Start()
    {
        cooldownBar = GetComponent<Image>();
        player = GameObject.Find("Alien").gameObject;
        playerController = player.GetComponent<PlayerController>();

        cooldownText = childText.GetComponent<TextMeshProUGUI>();
        cooldownText.text = maxCooldownTime.ToString("F2");

        // Update the cooldown values from the PlayerController
        invisibilityCooldown = playerController.invisibilityCooldown;
        maxCooldownTime = playerController.maxInvisCooldown;

        cooldownBar.color = endColor;
    }

    void Update()
    {
        // Update the cooldown value from the PlayerController
        invisibilityCooldown = playerController.invisibilityCooldown;

        // Check if the cooldown is greater than 0 before updating the bar
        if (invisibilityCooldown > 0)
        {
            UpdateCooldownBar();
        }
    }

    /// <summary>
    /// Changes the color of the cooldown bar
    /// </summary>
    void UpdateCooldownBar()
    {
        cooldownText.text = invisibilityCooldown.ToString("F2");
        float fillAmount = Mathf.Clamp01(invisibilityCooldown / maxCooldownTime);
        cooldownBar.fillAmount = fillAmount;

        if (fillAmount >= 1)
        {
            cooldownBar.color = Color.blue;
        }
        // Interpolate color based on fillAmount
        else if (fillAmount > 0)  // Only use Lerp when fillAmount is greater than 0
        {
            cooldownBar.color = Color.Lerp(startColor, endColor, fillAmount);
        }
        else  // Set to startColor when fillAmount is 0
        {
            cooldownBar.color = Color.blue;
        }
    }
}
