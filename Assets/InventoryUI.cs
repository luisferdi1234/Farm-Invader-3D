using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI gadgetEnergy;
    [SerializeField] private GameObject cloak;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject cow;

    Inventory inventory;
    int currentItemCount = 1;

    private void Start()
    {
        inventory = GameObject.Find("Alien").GetComponent<Inventory>();
    }

    private void Update()
    {
        if (inventory.inventorySlots[inventory.currentInventorySlot, 0] == null)
        {
            itemName.text = "None";
            gadgetEnergy.text = "";
            cloak.SetActive(false);
            apple.SetActive(false);
            cow.SetActive(false);
            currentItemCount = 1;
        }
        else if (inventory.inventorySlots[inventory.currentInventorySlot, 0] != null)
        {
            Item currentItem = inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>();

            if (currentItem.itemName == "Invisibility Cloak")
            {
                cloak.SetActive(true);
                apple.SetActive(false);
                cow.SetActive(false);
            }
            else if (currentItem.itemName == "Apple")
            {
                apple.SetActive(true);
                cloak.SetActive(false);
                cow.SetActive(false);
            }
            else if (currentItem.itemName == "Cow")
            {
                apple.SetActive(false);
                cloak.SetActive(false);
                cow.SetActive(true);
            }
            else
            {
                apple.SetActive(false);
                cloak.SetActive(false);
                cow.SetActive(false);
            }
            currentItemCount = 1;
            if (currentItem.stackable)
            {
                for (int i = 1; i < inventory.inventorySlots.GetLength(1); i++)
                {
                    if (inventory.inventorySlots[inventory.currentInventorySlot, i] != null)
                    {
                        currentItemCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                itemName.text = currentItem.itemName + $" ({currentItemCount})";
            }
            else
            {
                itemName.text = currentItem.itemName;
            }
            if (currentItem.isRechargeable)
            {
                gadgetEnergy.text = "Gadget Energy: ";
            }
            else
            {
                gadgetEnergy.text = "";
            }
        }
    }
}
