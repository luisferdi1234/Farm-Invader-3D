using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cow : MonoBehaviour
{
    [SerializeField] GameObject foodLove;
    public SphereCollider sphereCollider;
    public NavMeshAgent agent;
    public GameObject gemApple;

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
        if (gemApple == null)
        {
            if (other.gameObject.name.Contains("Apple") || other.gameObject.name.Contains("Alien") && other.gameObject.GetComponent<PlayerController>().heldItem != null && other.gameObject.GetComponent<PlayerController>().heldItem.name.Contains("Apple"))
            {
                gemApple = Instantiate(foodLove, transform.position + transform.forward * -1f, transform.rotation);
                gemApple.transform.parent = transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gemApple != null)
        {
            if (other.gameObject.name.Contains("Apple") || other.gameObject.name.Contains("Alien") && other.gameObject.GetComponent<PlayerController>().heldItem != null && other.gameObject.GetComponent<PlayerController>().heldItem.name.Contains("Apple"))
            {
                Destroy(gemApple);
            }
        }
    }
}
