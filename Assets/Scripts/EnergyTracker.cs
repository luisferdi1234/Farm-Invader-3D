using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyTracker : MonoBehaviour
{
    [SerializeField] GameObject energyBar;
    [SerializeField] GameObject fillMask;
    [SerializeField] GameObject fillImage;
    [SerializeField] GameObject overlayImage;

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
                if (energy < currentItem.maxEnergy)
                {
                    SetBarColor(Color.white);
                }
                else
                {
                    Color babyBlue = new Color(0.54f, 0.81f, 0.94f);
                    SetBarColor(babyBlue);
                }
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

    private void SetBarColor(Color newBarColor)
    {
        fillMask.GetComponent<Image>().color = newBarColor;
        fillImage.GetComponent<RawImage>().color = newBarColor;
        overlayImage.GetComponent<RawImage>().color = newBarColor;
    }
}
