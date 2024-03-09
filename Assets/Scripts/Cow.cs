using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cow : MonoBehaviour
{
    [SerializeField] GameObject foodLove;
    [HideInInspector] public SphereCollider sphereCollider;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public GameObject gemApple;
    private GameObject closestApple;

    private void Start()
    {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (agent.isActiveAndEnabled && closestApple == null && agent.destination != transform.position)
        {
            agent.SetDestination(transform.position);
            Destroy(gemApple);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (gemApple != null && other.gameObject.name.Contains("Apple") && other.CompareTag("Item") && gameObject.transform.parent.CompareTag("Item") && closestApple == null)
        {
            Debug.Log("Apple Detected!");
            AudioManager.instance.PlayRandomAudioClip("cowSounds");
            closestApple = other.gameObject;
            agent.SetDestination(closestApple.transform.position);
            
        }
        else if (gemApple == null)
        {
            //Checks if player is currently holding an apple, and is in range of cow
            if (other.gameObject.name.Contains("Apple") || other.gameObject.name.Contains("Alien") && other.gameObject.GetComponent<Inventory>().inventorySlots[other.gameObject.GetComponent<Inventory>().currentInventorySlot, 0] != null && other.gameObject.GetComponent<Inventory>().inventorySlots[other.gameObject.GetComponent<Inventory>().currentInventorySlot, 0].name.Contains("Apple"))
            {
                AudioManager.instance.PlayRandomAudioClip("cowSounds");
                gemApple = Instantiate(foodLove, transform.position + transform.forward + transform.up * 1.5f, transform.rotation);
                gemApple.transform.parent = transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gemApple != null)
        {
            if (other.gameObject.name.Contains("Alien") && other.gameObject.GetComponent<Inventory>().hasApple)
            {
                closestApple = null;
                Destroy(gemApple);
            }
        }
    }
}
