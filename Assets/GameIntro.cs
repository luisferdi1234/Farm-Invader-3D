using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    [SerializeField] GameObject menu;
    public void FadeOutCompleted()
    {
        menu.SetActive(true);
    }
}
