using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI gadgetEnergy;
    [SerializeField] private GameObject cloak;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject cow;
    [SerializeField] private GameObject holoBush;
    [SerializeField] private GameObject emp;

    Inventory inventory;
    int currentItemCount = 1;

    private void Start()
    {
        inventory = GameObject.Find("Alien").GetComponent<Inventory>();
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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

            //Turns on spinning game object in UI
            TurnOnUIGameObject(currentItem);

            //Shows how many of each item we have
            UpdateAmountOfItems(currentItem);
        }
    }

    /// <summary>
    /// Updates the UI to show how many items we currently are holding
    /// </summary>
    /// <param name="currentItem"></param>
    private void UpdateAmountOfItems(Item currentItem)
    {
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
            itemName.text = currentItem.itemName + $" ({currentItemCount} / 3)";
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

    /// <summary>
    /// Turns on spinning game object in UI
    /// </summary>
    /// <param name="currentItem"></param>
    private void TurnOnUIGameObject(Item currentItem)
    {
        if (currentItem.itemName == "Invisibility Cloak")
        {
            cloak.SetActive(true);
            apple.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(false);
        }
        else if (currentItem.itemName == "Apple")
        {
            apple.SetActive(true);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(false);
        }
        else if (currentItem.itemName == "Cow")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            holoBush.SetActive(false);
            cow.SetActive(true);
            emp.SetActive(false);
        }
        else if (currentItem.itemName == "Holo Bush")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(true);
            emp.SetActive(false);
        }
        else if (currentItem.itemName == "EMP")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(true);
        }
        else
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(false);
        }
    }

    //void CheckConnectedControllers()
    //{
    //    bool keyboardUsed = InputSystem.GetDevice<Keyboard>().anyKey.isPressed;
    //    bool mouseUsed = InputSystem.GetDevice<Mouse>().leftButton.isPressed || InputSystem.GetDevice<Mouse>().rightButton.isPressed;

    //    if (keyboardUsed || mouseUsed)
    //    {
    //        DisplayPrompt("Use WASD to move", movementText);
    //        DisplayPrompt("Use 'Left Click' to pick up items and cows, and Q and E to swap inventory", pickUpText);
    //        DisplayPrompt("Use 'Right Click' to use gadgets", gadgetText);
    //        DisplayPrompt("Use 'Space' to recharge gadget while next to a cow and to extract from level (while near a UFO)", rechargeText);
    //    }
    //    else
    //    {
    //        foreach (var device in InputSystem.devices)
    //        {
    //            Debug.Log("Device: " + device.name);
    //            if (device is Gamepad gamepad)
    //            {
    //                // Check for specific controllers based on name
    //                if (gamepad.name.Contains("Xbox")) // Xbox controller
    //                {
    //                    DisplayPrompt("Use Left Stick to move", movementText);
    //                    DisplayPrompt("Use 'A' to pick up items and cows", pickUpText);
    //                    DisplayPrompt("Use 'X' to use gadgets, and L1 and R1 to swap inventory", gadgetText);
    //                    DisplayPrompt("Use 'B' to recharge gadgets while next to a cow and to extract from level (while near a UFO)", rechargeText);
    //                }
    //                else if (gamepad.name.Contains("Sony") || gamepad.name.Contains("DualSense")) // PS4 or PS5 controller
    //                {
    //                    DisplayPrompt("Use Left Stick to move", movementText);
    //                    DisplayPrompt("Use 'X' to pick up items and cows, and L1 and R1 to swap inventory", pickUpText);
    //                    DisplayPrompt("Use 'Square' to use gadgets", gadgetText);
    //                    DisplayPrompt("Use 'Circle' to recharge gadgets while next to a cow and to extract from level (while near a UFO)", rechargeText);
    //                }
    //            }
    //        }
    //    }
    //}

    //void DisplayPrompt(string message, TMPro.TextMeshProUGUI TMPtext)
    //{
    //    TMPtext.text = message;
    //}
}
