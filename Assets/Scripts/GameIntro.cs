using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject cow;
    [SerializeField] GameObject introSkipper;
    public void FadeOutCompleted()
    {
        if (introSkipper.active)
        {
            introSkipper.SetActive(false);
        }
        menu.SetActive(true);
        cow.SetActive(true);
    }
}
