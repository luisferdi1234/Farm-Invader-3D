using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTracker : MonoBehaviour
{
    [SerializeField] GameObject energyBar;
    private float energy = 3f;
    ProgressBar bar;
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        bar = energyBar.GetComponent<ProgressBar>();
        bar.SetProgress(1);
        GameObject Alien = GameObject.Find("Alien");
        inventory = Alien.GetComponent<Inventory>();
    }

    private void Update()
    {
        Item currentItem;
        if (inventory != null && inventory.inventorySlots[inventory.currentInventorySlot, 0] != null)
        {
            currentItem = inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>();

            if (currentItem.isRechargeable)
            {
                energyBar.SetActive(true);
                energy = currentItem.energy;
                bar.SetProgress(energy/currentItem.maxEnergy);
            }
            else
            {
                energyBar.SetActive(false);
            }
        }
        else
        {
            energyBar.SetActive(false);
        }
    }
}
