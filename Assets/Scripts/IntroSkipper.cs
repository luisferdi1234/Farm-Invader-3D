using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class IntroSkipper : MonoBehaviour
{
    PlayerControls playerControls;

    InputAction skipIntro;
    InputAction click;

    [SerializeField] GameObject intro;
    [SerializeField] GameObject play;
    [SerializeField] GameObject howToPlay;
    [SerializeField] GameObject exit;

    [SerializeField] TextMeshProUGUI playText;
    [SerializeField] TextMeshProUGUI howToPlayText;
    [SerializeField] TextMeshProUGUI exitText;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        skipIntro = playerControls.UI.Submit;
        skipIntro.Enable();
        skipIntro.performed += SkipIntro;

        click = playerControls.UI.Click;
        click.Enable();
        click.performed += SkipIntro;
    }

    private void OnDisable()
    {
        skipIntro.Disable();

        click.Disable();
    }

    /// <summary>
    /// Turns off the intro.
    /// </summary>
    /// <param name="context"></param>
    private void SkipIntro (InputAction.CallbackContext context)
    {
        //Turns off intro
        intro.GetComponent<Animator>().enabled = false;
        intro.GetComponent<Image>().enabled = false;
        intro.GetComponent<GameIntro>().FadeOutCompleted();

        //Turns on Menu Buttons
        play.GetComponent<Image>().color = new Color(play.GetComponent<Image>().color.r, play.GetComponent<Image>().color.g, play.GetComponent<Image>().color.b, 1f);
        howToPlay.GetComponent<Image>().color = new Color(howToPlay.GetComponent<Image>().color.r, howToPlay.GetComponent<Image>().color.g, howToPlay.GetComponent<Image>().color.b, 1f);
        exit.GetComponent<Image>().color = new Color(exit.GetComponent<Image>().color.r, exit.GetComponent<Image>().color.g, exit.GetComponent<Image>().color.b, 1f);

        //Turns on Menu Button Text
        playText.color = new Color(playText.GetComponent<TextMeshProUGUI>().color.r, playText.GetComponent<TextMeshProUGUI>().color.g, playText.GetComponent<TextMeshProUGUI>().color.b, 1f);
        howToPlayText.color = new Color(howToPlayText.GetComponent<TextMeshProUGUI>().color.r, howToPlayText.GetComponent<TextMeshProUGUI>().color.g, howToPlayText.GetComponent<TextMeshProUGUI>().color.b, 1f);
        exitText.color = new Color(exitText.GetComponent<TextMeshProUGUI>().color.r, exitText.GetComponent<TextMeshProUGUI>().color.g, exitText.GetComponent<TextMeshProUGUI>().color.b, 1f);

        //Turns off the intro skipper
        gameObject.SetActive(false);
    }
}
