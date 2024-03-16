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

    [SerializeField] GameObject cloak;

    //Grabs game objects to allow items to look like they're being carried
    [SerializeField] GameObject spine;
    [SerializeField] GameObject rightHand;

    //Item variables
    public GameObject nearestItem;
    public float itemRadius;
    public int currentInventorySlot = 0;
    private int prevInventorySlot = 0;
    [HideInInspector] public bool hasApple = false;

    //Input System Variable
    PlayerControls playerControls;
    private InputAction fire;
    private InputAction inventoryRight;
    private InputAction inventoryLeft;

    //PlayerController script
    private PlayerController playerController;

    //Cloak variable
    public InvisibilityCloakManager invisibilityCloakManager;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        inventorySlots[0, 0] = cloak;
    }
    /// <summary>
    /// Enables Input System variables
    /// </summary>
    private void OnEnable()
    {
        playerController = GetComponent<PlayerController>();

        //Input System
        playerControls = new PlayerControls();

        //Input System
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        inventoryRight = playerControls.Player.InventoryRight;
        inventoryRight.Enable();
        inventoryRight.performed += InventoryRight;

        inventoryLeft = playerControls.Player.InventoryLeft;
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
        if (playerController.canMove)
        {
            //Check if cow is the nearby object
            if (inventorySlots[3, 0] == null && nearestItem != null && nearestItem.name.Contains("Cow"))
            {
                if (inventorySlots[currentInventorySlot, 0] != null)
                {
                    if (invisibilityCloakManager != null)
                    {
                        invisibilityCloakManager.TurnOffInvisibility();
                    }
                    inventorySlots[currentInventorySlot, 0].SetActive(false);
                }
                AudioManager.instance.PauseAlienCharge();
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
                        GrabItem(i, 0);
                        inventorySlots[prevInventorySlot, 0].SetActive(false);
                        inventorySlots[currentInventorySlot, 0].SetActive(true);
                        break;
                    }
                    //Else, check to see if item can be stacked
                    else if (inventorySlots[i, 0].GetComponent<Item>().itemName == nearestItem.GetComponent<Item>().itemName && nearestItem.GetComponent<Item>().stackable)
                    {
                        for (int j = 1; j < inventorySlots.GetLength(1); j++)
                        {
                            if (inventorySlots[i, j] == null)
                            {
                                GrabItem(i, j);
                                inventorySlots[i, j].SetActive(false);
                                inventorySlots[currentInventorySlot, 0].SetActive(true);

                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else if (currentInventorySlot != 0 && inventorySlots[currentInventorySlot, 0] != null)
            {
                ReleaseItem();
            }
            Debug.Log("Slot: " + currentInventorySlot + " Item: " + inventorySlots[currentInventorySlot, 0]);
        }
    }

    /// <summary>
    /// Handles the inventory right button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void InventoryRight(InputAction.CallbackContext context)
    {
        if (inventorySlots[3, 0] == null && ChangeToActiveRightSlot())
        {
            //Handles turning off current item
            if (inventorySlots[prevInventorySlot, 0] != null)
            {
                inventorySlots[prevInventorySlot, 0].SetActive(false);
            }

            //Handles turning on next item
            if (inventorySlots[currentInventorySlot, 0] != null)
            {
                inventorySlots[currentInventorySlot, 0].SetActive(true);
                ChangeItemRadius();
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
        if (inventorySlots[3,0] == null && ChangeToActiveLeftSlot())
        {
            //Turns off current item
            if (inventorySlots[prevInventorySlot, 0] != null)
            {
                inventorySlots[prevInventorySlot, 0].SetActive(false);
            }

            //Handles turning on next item
            if (inventorySlots[currentInventorySlot, 0] != null)
            {
                inventorySlots[currentInventorySlot, 0].SetActive(true);
                ChangeItemRadius();
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
        if (inventorySlots[currentInventorySlot, 0].GetComponent<Item>().itemName == "Holo Bush")
        {
            inventorySlots[currentInventorySlot, 0].transform.position = transform.position + transform.forward;
        }
        else
        {
            inventorySlots[currentInventorySlot, 0].transform.position = transform.position + transform.forward + (transform.up / 2) * itemRadius;
        }

        //Get all colliders attached to the
        Collider[] allColliders = inventorySlots[currentInventorySlot, 0].GetComponents<Collider>();
        // Enable each collider
        foreach (Collider collider in allColliders)
        {
            collider.enabled = true;
        }
        ChangeItemRadius();
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
            if (inventorySlots[currentInventorySlot, 0] == null && inventorySlots[prevInventorySlot, 0] == null)
            {
                currentInventorySlot = 0;
            }
            else if (inventorySlots[currentInventorySlot, 0] == null)
            {
                currentInventorySlot = prevInventorySlot;
            }
        }
        else
        {

            inventorySlots[currentInventorySlot, 0] = null;
            if (inventorySlots[prevInventorySlot, 0] == null)
            {
                currentInventorySlot = 0;
            }
            else
            {
                currentInventorySlot = prevInventorySlot;
            }
            if (inventorySlots[currentInventorySlot, 0] != null && !inventorySlots[currentInventorySlot, 0].active)
            {
                inventorySlots[currentInventorySlot, 0].SetActive(true);
            }
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
        prevInventorySlot = currentInventorySlot;
        currentInventorySlot = i;
        // Parent the new nearest item to the player
        inventorySlots[i,j] = nearestItem;
        nearestItem = null;
        inventorySlots[i, j].tag = "HeldItem";
        inventorySlots[i, j].GetComponent<Outline>().enabled = false;
        ChangeItemRadius();

        //Changes variables for if the item is a cow
        if (inventorySlots[i, j].name.Contains("Cow"))
        {
            PickUpCow(inventorySlots[i, j]);
            inventorySlots[i, j].GetComponent<CowItem>().itemDetector.GetComponent<SphereCollider>().enabled = false;

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

        //Turns off colliders attached to these objects
        if (inventorySlots[i, j].GetComponent<Item>().sphereCollider != null)
        {
            inventorySlots[i, j].GetComponent<Item>().sphereCollider.enabled = false;
        }
        else if (inventorySlots[i, j].GetComponent<Item>().capsuleCollider != null)
        {
            inventorySlots[i, j].GetComponent<Item>().capsuleCollider.enabled = false;
        }
    }

    /// <summary>
    /// Handles picking up the cow object
    /// </summary>
    private void PickUpCow(GameObject slot)
    {
        animator.SetBool("CarryingCow", true);
        slot.GetComponent<NavMeshAgent>().enabled = false;
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
        inventorySlots[3, 0].GetComponent<NavMeshAgent>().enabled = true;
        inventorySlots[3, 0].GetComponent<NavMeshAgent>().SetDestination(inventorySlots[3,0].transform.position);
        inventorySlots[3, 0].GetComponent<CowItem>().itemDetector.GetComponent<SphereCollider>().enabled = true;
        animator.SetBool("CarryingCow", false);
        playerController.moveSpeed = playerController.maxMoveSpeed;
    }

    /// <summary>
    /// Updates the radius of the item in the current inventory slot
    /// </summary>
    private void ChangeItemRadius()
    {
        if (inventorySlots[currentInventorySlot, 0].GetComponent<Item>().itemName == "Cow")
        {
            itemRadius = inventorySlots[currentInventorySlot, 0].GetComponent<CapsuleCollider>().radius;
        }
        else if (inventorySlots[currentInventorySlot, 0].GetComponent<Item>().itemName == "Invisibility Cloak")
        {
            itemRadius = 0;
        }
        else
        {
            itemRadius = inventorySlots[currentInventorySlot, 0].GetComponent<SphereCollider>().radius;
        }
    }

    /// <summary>
    /// Finds an inventory slot to the right and sets the current slot to that.
    /// </summary>
    private bool ChangeToActiveRightSlot()
    {
        for (int i = currentInventorySlot; i < inventorySlots.GetLength(0) - 1; i++)
        {
            if (i + 1 < inventorySlots.GetLength(0) - 1 && inventorySlots[i + 1, 0] != null)
            {
                prevInventorySlot = currentInventorySlot;
                currentInventorySlot = i + 1;
                return true;
            }
            //Cloak is always an active slot, so worst case go to cloak
            if (i == 2 && currentInventorySlot != 0)
            {
                prevInventorySlot = currentInventorySlot;
                currentInventorySlot = 0;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Finds an inventory slot to the left and sets the current slot to that.
    /// </summary>
    private bool ChangeToActiveLeftSlot()
    {
        int iterations = 0;
        for (int i = currentInventorySlot; i > -1; i--)
        {
            if (i == 0)
            {
                if (iterations == 1)
                {
                    return false;
                }
                i = 3;
                iterations++;
            }
            if (inventorySlots[i - 1, 0] != null)
            {
                prevInventorySlot = currentInventorySlot;
                currentInventorySlot = i - 1;
                return true;
            }
        }
        return false;
    }
}
