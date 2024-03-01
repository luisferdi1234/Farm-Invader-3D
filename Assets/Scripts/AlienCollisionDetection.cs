using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlienCollisionDetection : MonoBehaviour
{
    Inventory inventory;
    private float idleTimer = 0f;
    private void Start()
    {
        inventory = transform.parent.GetComponent<Inventory>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (inventory.heldItem == null && other.CompareTag("Item"))
        {
            other.GetComponent<Outline>().enabled = true;
            if (other.name.Contains("Cow"))
            {
                inventory.nearestItem = other.gameObject;
                inventory.itemRadius = other.GetComponent<CapsuleCollider>().radius;
            }
            else if (other.name.Contains("Apple"))
            {
                inventory.nearestItem = other.gameObject;
                inventory.itemRadius = other.GetComponent<SphereCollider>().radius;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inventory.heldItem == null && other.CompareTag("Item"))
        {
            other.GetComponent<Outline>().enabled = false;
            inventory.nearestItem = null;
            if(other.GetComponent<Item>().lightning != null && other.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().isPlaying)
            {
                other.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
                AudioManager.instance.PauseAlienCharge();
            }
        }
    }
}
