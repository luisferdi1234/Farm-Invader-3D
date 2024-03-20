using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloBush : MonoBehaviour
{
    [SerializeField] float maxTime = 15f;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= maxTime)
        {
            Destroy(gameObject);
        }
    }
}
