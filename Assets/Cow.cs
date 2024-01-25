using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cow : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] NavMeshAgent agent;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Apple" && other.CompareTag("Item"))
        {
            Debug.Log("Apple Detected!");
            agent.SetDestination(other.gameObject.transform.position);
        }
        else if (other.gameObject.name == "Alien" && Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position) <= 2f)
        {
            PlayerController pController = other.gameObject.GetComponent<PlayerController>();
            pController.nearestItem = gameObject;
            pController.itemRadius = GetComponent<CapsuleCollider>().radius;
        }
        else if(other.gameObject.name == "Alien" && Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position) >= 2f)
        {
            PlayerController pController = other.gameObject.GetComponent<PlayerController>();
            pController.nearestItem = null;
            pController.itemRadius = 0;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && collision.gameObject.name == "Apple")
        {
            Destroy(collision.gameObject);
        }
    }


}
