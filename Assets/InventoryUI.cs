using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI gadgetEnergy;

    Inventory inventory;

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
        }
        else if (inventory.inventorySlots[inventory.currentInventorySlot, 0] != null)
        {
            Item currentItem = inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>();
            itemName.text = currentItem.itemName;
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
