using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI gadgetEnergy;
    [SerializeField] private GameObject cloak;
    [SerializeField] private GameObject clone;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject cow;
    [SerializeField] private GameObject holoBush;
    [SerializeField] private GameObject emp;

    //Button Prompt UI Images
    [SerializeField] GameObject Fire;
    [SerializeField] GameObject Recharge;
    [SerializeField] GameObject UseAbility;
    [SerializeField] GameObject InventoryRight;
    [SerializeField] GameObject InventoryLeft;
    [SerializeField] TextMeshProUGUI PickUpText;

    //Mouse and Keyboard Sprites
    [SerializeField] Sprite LMB;
    [SerializeField] Sprite RMB;
    [SerializeField] Sprite Space;
    [SerializeField] Sprite Q;
    [SerializeField] Sprite E;

    //PS Sprites
    [SerializeField] Sprite Cross;
    [SerializeField] Sprite Square;
    [SerializeField] Sprite Circle;
    [SerializeField] Sprite PSR1;
    [SerializeField] Sprite PSL1;

    //Xbox Sprites
    [SerializeField] Sprite A;
    [SerializeField] Sprite X;
    [SerializeField] Sprite B;
    [SerializeField] Sprite XboxR1;
    [SerializeField] Sprite XboxL1;

    [SerializeField] GameObject mobileControls;
    [SerializeField] GameObject eventSystem;

    Inventory inventory;
    int currentItemCount = 1;
    private bool isMobile;

    private void Start()
    {
        inventory = GameObject.Find("Alien").GetComponent<Inventory>();
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        isMobile = Application.isMobilePlatform;
        if (isMobile)
        {
            TurnOffControlImages();
        }
        else
        {
            CheckConnectedControllers();
            mobileControls.SetActive(false);
            eventSystem.SetActive(false);
        }
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
            Fire.SetActive(false);
            Recharge.SetActive(false);
            UseAbility.SetActive(false);
        }
        else if (inventory.inventorySlots[inventory.currentInventorySlot, 0] != null)
        {
            //Changes pick up text
            if (inventory.nearestItem != null)
            {
                Fire.SetActive(true);
                PickUpText.text = "Pick Up";
            }
            else if (inventory.currentInventorySlot != 0)
            {
                Fire.SetActive(true);
                PickUpText.text = "Put Down";
            }
            else
            {
                PickUpText.text = " ";
                Fire.SetActive(false);
            }

            Item currentItem = inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>();

            //Shows how to use ability when max charge, and shows how to recharge
            if (currentItem.itemName == "Cow")
            {
                if (UseAbility.active)
                {
                    UseAbility.SetActive(false);
                }
                if (Recharge.active)
                {
                    Recharge.SetActive(false);
                }
                if (InventoryLeft.active)
                {
                    InventoryLeft.SetActive(false);
                }
                if (InventoryRight.active)
                {
                    InventoryRight.SetActive(false);
                }
            }
            else if (currentItem.isRechargeable && currentItem.energy >= currentItem.maxEnergy)
            {
                if (!UseAbility.active)
                {
                    UseAbility.SetActive(true);
                }
                if (Recharge.active)
                {
                    Recharge.SetActive(false);
                }
                if (!InventoryLeft.active)
                {
                    InventoryLeft.SetActive(true);
                }
                if (!InventoryRight.active)
                {
                    InventoryRight.SetActive(true);
                }
            }
            else if (currentItem.isRechargeable && inventory.nearestItem != null && inventory.nearestItem.GetComponent<Item>().itemName == "Cow")
            {
                if (UseAbility.active)
                {
                    UseAbility.SetActive(false);
                }
                if (!Recharge.active)
                {
                    Recharge.SetActive(true);
                }
                if (!InventoryLeft.active)
                {
                    InventoryLeft.SetActive(true);
                }
                if (!InventoryRight.active)
                {
                    InventoryRight.SetActive(true);
                }
            }
            else
            {
                if (UseAbility.active)
                {
                    UseAbility.SetActive(false);
                }
                if (Recharge.active)
                {
                    Recharge.SetActive(false);
                }
                if (!InventoryLeft.active)
                {
                    InventoryLeft.SetActive(true);
                }
                if (!InventoryRight.active)
                {
                    InventoryRight.SetActive(true);
                }
            }

            //Turns on spinning game object in UI
            TurnOnUIGameObject(currentItem);

            //Shows how many of each item we have
            UpdateAmountOfItems(currentItem);
        }
    }

    /// <summary>
    /// Turns off the on screen control images
    /// </summary>
    private void TurnOffControlImages()
    {
        Image fireImage = Fire.GetComponent<Image>();
        fireImage.enabled = false;

        Image rechargeImage = Recharge.GetComponent<Image>();
        rechargeImage.enabled = false;

        Image useAbilityImage = UseAbility.GetComponent<Image>();
        useAbilityImage.enabled = false;

        Image inventoryRightImage = InventoryRight.GetComponent<Image>();
        inventoryRightImage.enabled = false;

        Image inventoryLeftImage = InventoryLeft.GetComponent<Image>();
        inventoryLeftImage.enabled = false;
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
            clone.SetActive(false);
        }
        else if (currentItem.itemName == "Apple")
        {
            apple.SetActive(true);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(false);
            clone.SetActive(false);
        }
        else if (currentItem.itemName == "Cow")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            holoBush.SetActive(false);
            cow.SetActive(true);
            emp.SetActive(false);
            clone.SetActive(false);
        }
        else if (currentItem.itemName == "Holo Bush")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(true);
            emp.SetActive(false);
            clone.SetActive(false);
        }
        else if (currentItem.itemName == "EMP")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(true);
            clone.SetActive(false);
        }
        else if (currentItem.itemName == "Doppleganger Decoy")
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(false);
            clone.SetActive(true);
        }
        else
        {
            apple.SetActive(false);
            cloak.SetActive(false);
            cow.SetActive(false);
            holoBush.SetActive(false);
            emp.SetActive(false);
            clone.SetActive(false);
        }
    }

    void CheckConnectedControllers()
    {
        bool keyboardUsed = InputSystem.GetDevice<Keyboard>().anyKey.isPressed;
        bool mouseUsed = InputSystem.GetDevice<Mouse>().leftButton.isPressed || InputSystem.GetDevice<Mouse>().rightButton.isPressed;

        Image fireImage = Fire.GetComponent<Image>();
        Image rechargeImage = Recharge.GetComponent<Image>();
        Image useAbilityImage = UseAbility.GetComponent<Image>();
        Image inventoryRightImage = InventoryRight.GetComponent<Image>();
        Image inventoryLeftImage = InventoryLeft.GetComponent<Image>();

        if (keyboardUsed || mouseUsed)
        {
            DisplayPrompt(fireImage, LMB);
            DisplayPrompt(rechargeImage, Space);
            DisplayPrompt(useAbilityImage, RMB);
            DisplayPrompt(inventoryRightImage, E);
            DisplayPrompt(inventoryLeftImage, Q);
        }
        else
        {
            foreach (var device in InputSystem.devices)
            {
                Debug.Log("Device: " + device.name);
                if (device is Gamepad gamepad)
                {
                    // Check for specific controllers based on name
                    if (gamepad.name.Contains("Xbox")) // Xbox controller
                    {
                        DisplayPrompt(fireImage, A);
                        DisplayPrompt(rechargeImage, B);
                        DisplayPrompt(useAbilityImage, X);
                        DisplayPrompt(inventoryLeftImage, XboxL1);
                        DisplayPrompt(inventoryRightImage, XboxR1);
                    }
                    else if (gamepad.name.Contains("Sony") || gamepad.name.Contains("DualSense")) // PS4 or PS5 controller
                    {
                        DisplayPrompt(fireImage, Cross);
                        DisplayPrompt(rechargeImage, Circle);
                        DisplayPrompt(useAbilityImage, Square);
                        DisplayPrompt(inventoryLeftImage, PSL1);
                        DisplayPrompt(inventoryRightImage, PSR1);
                    }
                }
            }
        }
    }

    void DisplayPrompt(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }
}
