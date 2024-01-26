using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowCollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && collision.gameObject.name.Contains("Apple"))
        {
            Destroy(collision.gameObject);
        }
    }
}
