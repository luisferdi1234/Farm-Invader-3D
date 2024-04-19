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
    Animator cowAnimator;
    float idleTimer = 0f;
    float maxIdleTimer = 1f;

    private void Start()
    {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
        inventory  = GameObject.Find("Alien").gameObject.GetComponent<Inventory>(); 
        cowAnimator = transform.parent.GetComponent<Animator>();   
    }

    private void Update()
    {
        //Gets rid of gem apple upon picking cow up
        if (!sphereCollider.enabled && closestApple != null)
        {
            closestApple = null;
            Destroy(gemApple);
            agent.SetDestination(transform.position);
        }
        //Makes it so that an apple in secondary slot makes cow stop moving.
        else if (closestApple != null && closestApple.transform.parent != null && closestApple.transform.parent.tag.Contains("Player"))
        {
            closestApple = null;
            Destroy(gemApple);
            agent.SetDestination(transform.position);
            if (!cowAnimator.enabled)
            {
                cowAnimator.enabled = true;
            }
            cowAnimator.SetBool("Walking", false);
        }
        //Makes cow stop walking towards a deleted apple
        else if (agent.isActiveAndEnabled && agent.hasPath && closestApple == null && gemApple != null)
        {
            Destroy(gemApple);
            agent.SetDestination(transform.position);
            if (!cowAnimator.enabled)
            {
                cowAnimator.enabled = true;
            }
            cowAnimator.SetBool("Walking", false);
        }
        //Makes gem apple disappear when it gets picked up
        else if (gemApple != null && inventory.hasApple && !inventory.inventorySlots[inventory.currentInventorySlot, 0].name.Contains("Apple") && closestApple == null)
        {
            Destroy(gemApple);
            if (!cowAnimator.enabled)
            {
                cowAnimator.enabled = true;
            }
            cowAnimator.SetBool("Walking", false);
        }
        //Waits for cow to be on ground for 1 second before turning on idle
        if (idleTimer >= maxIdleTimer && !cowAnimator.enabled)
        {
            cowAnimator.enabled = true;
            cowAnimator.SetBool("Walking", false);
            idleTimer = 0f;
        }
        //Starts idle count down when cow is on ground
        else if (!cowAnimator.enabled && sphereCollider.enabled)
        {
            idleTimer += Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (gemApple != null && agent.isActiveAndEnabled && other.gameObject.name.Contains("Apple") && other.CompareTag("Item") && gameObject.transform.parent.CompareTag("Item") && closestApple == null)
        {
            Debug.Log("Apple Detected!");
            AudioManager.instance.PlayRandomAudioClip("cowSounds");
            closestApple = other.gameObject;
            agent.SetDestination(closestApple.transform.position);
            if (!cowAnimator.enabled)
            {
                cowAnimator.enabled = true;
            }
            cowAnimator.SetBool("Walking", true);

        }
        if (gemApple == null)
        {
            //Checks if player is currently holding an apple, and is in range of cow
            if (other.gameObject.name.Contains("Apple") || (other.gameObject.name.Contains("Alien") && inventory.inventorySlots[inventory.currentInventorySlot, 0].name.Contains("Apple") && Vector3.Distance(other.transform.position, transform.position) <= sphereCollider.radius - .2f))
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
            if (other.gameObject.name.Contains("Alien") && closestApple == null)
            {
                Destroy(gemApple);
                if (!cowAnimator.enabled)
                {
                    cowAnimator.enabled = true;
                }
                cowAnimator.SetBool("Walking", false);
            }
        }
    }
}
