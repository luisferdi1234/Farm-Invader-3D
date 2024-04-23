using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMobileButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isMobilePlatform)
        {
            gameObject.SetActive(false);
        }
    }
}
