using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController pController = other.gameObject.GetComponent<PlayerController>();
        pController.nearestItem = gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pController = other.gameObject.GetComponent<PlayerController>();
        pController.nearestItem = null;
    }
}
