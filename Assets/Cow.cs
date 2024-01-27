using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cow : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public NavMeshAgent agent;

    private void Start()
    {
        agent = transform.parent.GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Apple") && other.CompareTag("Item") && gameObject.transform.parent.CompareTag("Item"))
        {
            Debug.Log("Apple Detected!");
            agent.SetDestination(other.gameObject.transform.position);
        }
    }
}
