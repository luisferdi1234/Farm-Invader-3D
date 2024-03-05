using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortPoint : MonoBehaviour
{
    GameObject canvas;
    private void Start()
    {
        canvas = GameObject.Find("WinScreenCanvas");
        canvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item" && other.gameObject.name.Contains("Cow"))
        {
            canvas.SetActive(true);
            canvas.GetComponent<WinScreenCanvas>().OnLevelWon();
        }
    }
}
