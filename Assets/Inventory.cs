using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    //Item array for inventory
    [HideInInspector] public Item[,] inventorySlots = new Item[4, 3];

    //Grabs game objects to allow items to look like they're being carried
    [SerializeField] GameObject spine;
    [SerializeField] GameObject rightHand;

    //Item variables
    [HideInInspector] public GameObject nearestItem;
    [HideInInspector] public GameObject heldItem;
    public float itemRadius;

    //Input System Variable
    private InputAction fire;

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
        fire = playerController.playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    /// <summary>
    /// Disables Input System variables
    /// </summary>
    private void OnDisable()
    {
        fire.Disable();
    }

    /// <summary>
    /// Updates position of item 
    /// </summary>
    private void FixedUpdate()
    {
        if (heldItem != null)
        {
            if (heldItem.name.Contains("Cow"))
            {
                heldItem.transform.position = spine.transform.position + spine.transform.forward * itemRadius * 2;
            }
            else
            {
                heldItem.transform.position = rightHand.transform.position;
            }
        }
    }
    /// <summary>
    /// Handles the fire button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void Fire(InputAction.CallbackContext context)
    {
        if (nearestItem != null)
        {
            if (heldItem == nearestItem.gameObject)
            {
                ReleaseItem();
            }
            else if (heldItem == null)
            {
                GrabItem();
            }
        }
        else if (heldItem != null)
        {
            ReleaseItem();
        }
    }
    /// <summary>
    /// Lets go of the item you are currently holding
    /// </summary>
    public void ReleaseItem()
    {
        if (heldItem.name.Contains("Cow"))
        {
            Cow cow = heldItem.GetComponent<Cow>();
            heldItem.GetComponent<NavMeshAgent>().enabled = true;
            heldItem.GetComponent<NavMeshAgent>().SetDestination(heldItem.transform.position);
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
            animator.SetBool("CarryingCow", false);
            playerController.moveSpeed = playerController.maxMoveSpeed;
        }
        heldItem.tag = "Item";
        nearestItem = null;
        heldItem.transform.parent = null;
        heldItem.transform.position = transform.position + transform.forward + (transform.up / 2) * itemRadius;
        //Get all colliders attached to the
        Collider[] allColliders = heldItem.GetComponents<Collider>();
        // Enable each collider
        foreach (Collider collider in allColliders)
        {
            collider.enabled = true;
        }
        heldItem = null;
        itemRadius = 0;
    }

    /// <summary>
    /// Grabs the nearest item
    /// </summary>
    public void GrabItem()
    {
        // Parent the new nearest item to the player
        heldItem = nearestItem.gameObject;
        nearestItem = null;
        heldItem.tag = "HeldItem";
        heldItem.GetComponent<Outline>().enabled = false;
        //Changes variables for if the item is a cow
        if (heldItem.name.Contains("Cow"))
        {
            animator.SetBool("CarryingCow", true);
            heldItem.GetComponent<NavMeshAgent>().enabled = false;
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            playerController.moveSpeed = playerController.cowSlowDown;
            // Set the initial relative rotation of the cow when picked up
            Vector3 relativeRotation = new Vector3(0f, transform.eulerAngles.y + 90f, 0f); // Adjust as needed
            heldItem.transform.localRotation = Quaternion.Euler(relativeRotation);
            heldItem.transform.parent = spine.transform;
            AudioManager.instance.PlayRandomAudioClip("cowSounds");
        }
        else
        {
            heldItem.transform.parent = rightHand.transform;
        }

        //Get all colliders attached to this GameObject
        Collider[] allColliders = heldItem.GetComponents<Collider>();

        // Disable each collider
        foreach (Collider collider in allColliders)
        {
            collider.enabled = false;
        }
    }
}
