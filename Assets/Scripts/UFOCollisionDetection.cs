using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOCollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Abducted" && other.gameObject.name.Contains("Cow"))
        {
            AudioManager.instance.PlaySpecificSound("Cow Abducted", 1f);
            Destroy(other.gameObject);
            ScoreManager.Instance.AddUpCows();
        }
    }
}
