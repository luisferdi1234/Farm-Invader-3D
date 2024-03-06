using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject cow;
    public void FadeOutCompleted()
    {
        menu.SetActive(true);
        cow.SetActive(true);
    }
}
