using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlienCollisionDetection : MonoBehaviour
{
    PlayerController playerController;
    private float idleTimer = 0f;
    private void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerController.heldItem == null && other.CompareTag("Item"))
        {
            if (other.name.Contains("Cow"))
            {
                playerController.nearestItem = other.gameObject;
                playerController.itemRadius = other.GetComponent<CapsuleCollider>().radius;
            }
            else if (other.name.Contains("Apple"))
            {
                playerController.nearestItem = other.gameObject;
                playerController.itemRadius = other.GetComponent<SphereCollider>().radius;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerController.heldItem == null)
        {
            playerController.nearestItem = null;
        }
    }
}
