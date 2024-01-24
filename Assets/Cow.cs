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
    }

    private void OnTriggerExit(Collider other)
    {
        agent.SetDestination(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && collision.gameObject.name == "Apple")
        {
            Destroy(collision.gameObject);
        }
    }


}
