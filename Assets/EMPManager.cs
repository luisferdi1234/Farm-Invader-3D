using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EMPManager : MonoBehaviour
{
    //Array variables for farmers and lights
    GameObject[] farmerLights = null;
    GameObject[] farmers = null;

    //Emp Variables
    float empTimer = 0;
    float maxEMPTime = 5f;
    bool lightsOff = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find all game objects with the tag "Farmer"
        farmers = GameObject.FindGameObjectsWithTag("Farmer");

        // Grabs all farmer lights
        farmerLights = GameObject.FindGameObjectsWithTag("FarmerLight");

    }

    /// <summary>
    /// Handles the turning off and on of the EMP
    /// </summary>
    private void Update()
    {
        if (lightsOff)
        {
            empTimer += Time.deltaTime;
            if (empTimer > maxEMPTime)
            {
                EMPOff();
            }
        }
    }

    /// <summary>
    /// Turns off vision and farmer lights
    /// </summary>
    public void EMPOn()
    {
        foreach (GameObject farmer in farmers)
        {
            farmer.GetComponent<Enemy>().hasVision = false;
        }

        foreach (GameObject light in farmerLights)
        {
            light.GetComponent<Light>().enabled = false;
        }
        lightsOff = true;
    }

    /// <summary>
    /// Turns on vision and farmer lights
    /// </summary>
    public void EMPOff()
    {
        foreach (GameObject farmer in farmers)
        {
            farmer.GetComponent<Enemy>().hasVision = true;
        }

        foreach (GameObject light in farmerLights)
        {
            light.GetComponent<Light>().enabled = true;
        }
        lightsOff = false;
    }
}
