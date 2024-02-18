using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTracker : MonoBehaviour
{
    private float energy = 5f;
    ProgressBar bar;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<ProgressBar>();
        bar.SetProgress(1);
        playerController = GameObject.Find("Alien").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController != null)
        {

            if (energy != playerController.Energy)
            {
                energy = playerController.Energy;
                bar.SetProgress(energy/playerController.maxEnergy);
            }
        }
    }
}
