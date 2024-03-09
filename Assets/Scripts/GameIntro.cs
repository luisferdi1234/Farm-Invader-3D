using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject cow;

    public void Start()
    {
        if (PlayerProgressManager.Instance.IntroPlayed)
        {
            GetComponent<Animator>().enabled = false;
            //GetComponent<Image>().
            FadeOutCompleted();
        }
    }
    public void FadeOutCompleted()
    {
        menu.SetActive(true);
        cow.SetActive(true);
    }
}
