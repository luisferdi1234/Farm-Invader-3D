using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortPoint : MonoBehaviour
{
    Canvas canvas;
    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Cow")
        {
            canvas.enabled = true;
        }
    }
}
