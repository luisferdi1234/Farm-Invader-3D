using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AppleEater : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && collision.gameObject.name.Contains("Apple"))
        {
            GameObject gemApple = gameObject.GetComponentInChildren<CowCollisionDetection>().gemApple;
            if (gemApple != null)
            {
                Destroy(gameObject.GetComponentInChildren<CowCollisionDetection>().gemApple);
            }
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().updatePosition = true;
            GetComponent<NavMeshAgent>().updateRotation = true;
            gameObject.GetComponent<Animator>().SetBool("Walking", false);
            AudioManager.instance.PlaySpecificSound("AppleCrunch", .5f);
            Destroy(collision.gameObject);
        }
    }
}
