using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowCollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") && collision.gameObject.name.Contains("Apple"))
        {
            GameObject gemApple = gameObject.GetComponentInChildren<Cow>().gemApple;
            if (gemApple != null)
            {
                Destroy(gameObject.GetComponentInChildren<Cow>().gemApple);
            }
            AudioManager.instance.PlaySpecificSound("AppleCrunch", .5f);
            Destroy(collision.gameObject);
        }
    }
}
