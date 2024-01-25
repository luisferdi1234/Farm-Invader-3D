using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController pController = other.gameObject.GetComponent<PlayerController>();
            pController.nearestItem = gameObject;
            pController.itemRadius = GetComponent<SphereCollider>().radius;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController pController = other.gameObject.GetComponent<PlayerController>();
            pController.nearestItem = null;
            pController.itemRadius = 0;
        }

    }
}
