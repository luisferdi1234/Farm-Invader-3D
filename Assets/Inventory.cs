using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    //Item array for inventory
    public GameObject[,] inventorySlots = new GameObject[4, 3];
    //0,0 slot is always for cloak
    //3,0 slot is always for cow

    //Grabs game objects to allow items to look like they're being carried
    [SerializeField] GameObject spine;
    [SerializeField] GameObject rightHand;

    //Item variables
    public GameObject nearestItem;
    public float itemRadius;
    public int currentInventorySlot = 0;
    [HideInInspector] public bool hasApple = false;

    //Input System Variable
    private InputAction fire;
    private InputAction inventoryRight;
    private InputAction inventoryLeft;

    //PlayerController script
    private PlayerController playerController;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// Enables Input System variables
    /// </summary>
    private void OnEnable()
    {
        playerController = GetComponent<PlayerController>();

        //Input System
        fire = playerController.playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        inventoryRight = playerController.playerControls.Player.InventoryRight;
        inventoryRight.Enable();
        inventoryRight.performed += InventoryRight;

        inventoryLeft = playerController.playerControls.Player.InventoryLeft;
        inventoryLeft.Enable();
        inventoryLeft.performed += InventoryLeft;
    }

    /// <summary>
    /// Disables Input System variables
    /// </summary>
    private void OnDisable()
    {
        fire.Disable();

        inventoryRight.Disable();

        inventoryLeft.Disable();
    }

    /// <summary>
    /// Updates position of item 
    /// </summary>
    private void FixedUpdate()
    {
        if (inventorySlots[currentInventorySlot,0] != null)
        {
            if (inventorySlots[currentInventorySlot, 0].name.Contains("Cow"))
            {
                inventorySlots[currentInventorySlot, 0].transform.position = spine.transform.position + spine.transform.forward * itemRadius * 2;
            }
            else
            {
                inventorySlots[currentInventorySlot, 0].transform.position = rightHand.transform.position;
            }
        }
    }
    /// <summary>
    /// Handles the fire button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void Fire(InputAction.CallbackContext context)
    {

        //Check if cow is the nearby object
        if (inventorySlots[3, 0] == null && nearestItem != null && nearestItem.name.Contains("Cow"))
        {
            currentInventorySlot = 3;
            GrabItem(3, 0);

        }
        else if (nearestItem != null && inventorySlots[3, 0] == null)
        {
            //Checks for an empty inventory slot
            for (int i = 1; i < inventorySlots.GetLength(0); i++)
            {
                //If item slot available put it in first slot
                if (inventorySlots[i, 0] == null)
                {
                    currentInventorySlot = i;
                    GrabItem(i, 0);
                    break;
                }
                //Else, check to see if item can be stacked
                else if (inventorySlots[i, 0].GetComponent<Item>().itemName == nearestItem.GetComponent<Item>().itemName)
                {
                    for (int j = 1; j < inventorySlots.GetLength(1); j++)
                    {
                        if (inventorySlots[i, j] == null)
                        {
                            currentInventorySlot = i;
                            GrabItem(i, j);
                            inventorySlots[i, j].SetActive(false);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        else if (inventorySlots[currentInventorySlot, 0] != null)
        {
            ReleaseItem();
        }
        Debug.Log("Slot: " + currentInventorySlot + " Item: " + inventorySlots[currentInventorySlot, 0]);
    }

    /// <summary>
    /// Handles the inventory right button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void InventoryRight(InputAction.CallbackContext context)
    {
        if (!inventorySlots[currentInventorySlot, 0].name.Contains("Cow"))
        {
            currentInventorySlot += 1;
            if (currentInventorySlot > inventorySlots.GetLength(0) - 1)
            {
                currentInventorySlot = 0;
            }
            Debug.Log("Slot: " + currentInventorySlot + " Item: " + inventorySlots[currentInventorySlot, 0]);
        }
    }

    /// <summary>
    /// Handles the inventory right button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void InventoryLeft(InputAction.CallbackContext context)
    {
        if (!inventorySlots[currentInventorySlot, 0].name.Contains("Cow"))
        {
            currentInventorySlot -= 1;
            if (currentInventorySlot <= 0)
            {
                currentInventorySlot = inventorySlots.GetLength(0) - 1;
            }
            Debug.Log("Slot: " + currentInventorySlot + " Item: " + inventorySlots[currentInventorySlot, 0]);
        }
    }

    /// <summary>
    /// Lets go of the item you are currently holding
    /// </summary>
    public void ReleaseItem()
    {
        //Handles dropping a cow
        if (inventorySlots[currentInventorySlot, 0].name.Contains("Cow"))
        {
            DropCow();
        }
        inventorySlots[currentInventorySlot, 0].tag = "Item";
        nearestItem = null;
        inventorySlots[currentInventorySlot, 0].transform.parent = null;
        inventorySlots[currentInventorySlot, 0].transform.position = transform.position + transform.forward + (transform.up / 2) * itemRadius;

        //Get all colliders attached to the
        Collider[] allColliders = inventorySlots[currentInventorySlot, 0].GetComponents<Collider>();
        // Enable each collider
        foreach (Collider collider in allColliders)
        {
            collider.enabled = true;
        }

        //Handles shifting the array if the item is stackable
        if (inventorySlots[currentInventorySlot, 0].GetComponent<Item>().stackable)
        {
            string itemName = inventorySlots[currentInventorySlot, 0].GetComponent<Item>().itemName;
            inventorySlots[currentInventorySlot, 0] = null;
            ShiftInventoryArray();

            //Changes has apple bool for cow code
            if(inventorySlots[currentInventorySlot, 0] == null && itemName == "Apple")
            {
                hasApple = false;
            }
        }
        else
        {
            inventorySlots[currentInventorySlot, 0] = null;
        }
        itemRadius = 0;
    }

    /// <summary>
    /// Shifts the array of items that are stackable
    /// </summary>
    private void ShiftInventoryArray()
    {
        if (inventorySlots[currentInventorySlot, 1] != null)
        {
            for (int i = 0; i < inventorySlots.GetLength(1) - 1; i++)
            {
                if (inventorySlots[currentInventorySlot, i + 1] == null)
                {
                    break;
                }
                inventorySlots[currentInventorySlot, i] = inventorySlots[currentInventorySlot, i + 1];
                inventorySlots[currentInventorySlot, i + 1] = null;
            }
            inventorySlots[currentInventorySlot, 0].SetActive(true);
        }
    }

    /// <summary>
    /// Grabs the nearest item
    /// </summary>
    public void GrabItem(int i, int j)
    {
        // Parent the new nearest item to the player
        inventorySlots[i,j] = nearestItem;
        nearestItem = null;
        inventorySlots[i, j].tag = "HeldItem";
        inventorySlots[i, j].GetComponent<Outline>().enabled = false;

        //Changes variables for if the item is a cow
        if (inventorySlots[i, j].name.Contains("Cow"))
        {
            PickUpCow(inventorySlots[i, j]);
        }
        else if (inventorySlots[i,j].name.Contains("Apple"))
        {
            hasApple = true;
            inventorySlots[currentInventorySlot, 0].transform.position = rightHand.transform.position;
            inventorySlots[i, j].transform.parent = rightHand.transform;
        }
        else
        {
            inventorySlots[currentInventorySlot, 0].transform.position = rightHand.transform.position;
            inventorySlots[i, j].transform.parent = rightHand.transform;
        }

        //Get all colliders attached to this GameObject
        Collider[] allColliders = inventorySlots[i, j].GetComponents<Collider>();

        // Disable each collider
        foreach (Collider collider in allColliders)
        {
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Handles picking up the cow object
    /// </summary>
    private void PickUpCow(GameObject slot)
    {
        animator.SetBool("CarryingCow", true);
        slot.GetComponent<NavMeshAgent>().enabled = false;
        slot.GetComponent<Rigidbody>().isKinematic = false;
        playerController.moveSpeed = playerController.cowSlowDown;
        // Set the initial relative rotation of the cow when picked up
        Vector3 relativeRotation = new Vector3(0f, transform.eulerAngles.y + 90f, 0f); // Adjust as needed
        slot.transform.localRotation = Quaternion.Euler(relativeRotation);
        slot.transform.parent = spine.transform;
        slot.transform.position = spine.transform.position + spine.transform.forward * itemRadius * 2;
        AudioManager.instance.PlayRandomAudioClip("cowSounds");
    }

    /// <summary>
    /// Handles dropping the cow object
    /// </summary>
    private void DropCow()
    {
        Cow cow = inventorySlots[3, 0].GetComponent<Cow>();
        inventorySlots[3, 0].GetComponent<NavMeshAgent>().enabled = true;
        inventorySlots[3, 0].GetComponent<NavMeshAgent>().SetDestination(inventorySlots[3,0].transform.position);
        inventorySlots[3, 0].GetComponent<Rigidbody>().isKinematic = true;
        animator.SetBool("CarryingCow", false);
        playerController.moveSpeed = playerController.maxMoveSpeed;
    }
}
