using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscortPoint : MonoBehaviour
{
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item" && other.gameObject.name.Contains("Cow"))
        {
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<AnimationScript>().enabled = true;
            other.tag = "Abducted";
            other.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 8, 0);
        }
        else if (other.gameObject.name.Contains("Alien"))
        {
            other.GetComponent<PlayerController>().inUFO = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Alien"))
        {
            other.GetComponent<PlayerController>().inUFO = false;
        }
    }
}
