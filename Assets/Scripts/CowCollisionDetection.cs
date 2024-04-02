using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowCollisionDetection : MonoBehaviour
{
    [SerializeField] GameObject foodLove;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public GameObject gemApple;
    private GameObject closestApple;
    private SphereCollider sphereCollider;
    Inventory inventory;

    private void Start()
    {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        sphereCollider = GetComponent<SphereCollider>();
        inventory  = GameObject.Find("Alien").gameObject.GetComponent<Inventory>(); 
    }

    private void Update()
    {
        //Gets rid of gem apple upon picking cow up
        if (!sphereCollider.enabled && closestApple != null)
        {
            agent.enabled = false;
            closestApple = null;
            Destroy(gemApple);
        }
        //Makes it so that an apple in secondary slot makes cow stop moving.
        else if (closestApple != null && closestApple.transform.parent != null && closestApple.transform.parent.tag.Contains("Player"))
        {
            agent.enabled = false;
            closestApple = null;
            Destroy(gemApple);
        }
        //Makes cow stop walking towards a deleted apple
        else if (agent.isActiveAndEnabled && agent.hasPath && closestApple == null && gemApple != null)
        {
            agent.enabled = false;
            Destroy(gemApple);
        }
        //Makes gem apple disappear when it gets picked up
        else if (gemApple != null && inventory.hasApple && !inventory.inventorySlots[inventory.currentInventorySlot, 0].name.Contains("Apple") && closestApple == null)
        {
            agent.enabled = false;
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
            agent.enabled = true;
            agent.SetDestination(closestApple.transform.position);
            
        }
        if (gemApple == null)
        {
            //Checks if player is currently holding an apple, and is in range of cow
            if (other.gameObject.name.Contains("Apple") || (other.gameObject.name.Contains("Alien") && inventory.inventorySlots[inventory.currentInventorySlot, 0].name.Contains("Apple")))
            {
                Debug.Log("Apple Nearby!");
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
            if (other.gameObject.name.Contains("Alien") && inventory.hasApple && closestApple == null)
            {
                agent.enabled = false;
                Destroy(gemApple);
            }
        }
    }
}
