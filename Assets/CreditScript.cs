using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    public void ReturnToMenu()
    {
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("Main Menu");
    }
}
