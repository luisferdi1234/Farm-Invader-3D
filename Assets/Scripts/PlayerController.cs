using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //SerializedFields
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float cowSlowDown = 2f;
    [SerializeField] float maxInvisTime = 3f;
    [SerializeField] Material alienSkin;

    //Item variables
    public GameObject nearestItem;
    public GameObject heldItem;
    public float itemRadius;

    //Invisibility variables
    public float invisTime = 0;
    public float invisibilityCooldown = 0f;
    public float maxInvisCooldown = 15f;
    private bool InvisInUse = false;
    private bool startInvisCooldown = false;
    //Cinemachine
    private CinemachineVirtualCamera vcam;

    //InputSystem Variables
    public PlayerControls playerControls;
    Vector3 moveDirection = Vector3.zero;
    private InputAction move;
    private InputAction fire;
    private InputAction invisible;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();

        //Cinemachine
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = gameObject.transform;
        vcam.LookAt = gameObject.transform;
    }

    private void OnEnable()
    {
        //Input System
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        invisible = playerControls.Player.Invisible;
        invisible.Enable();
        invisible.performed += Invisible;
    }

    private void OnDisable()
    {
        //Input System
        move.Disable();
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        UpdateInvisibility();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);

        // Calculate rotation to look at the movement direction
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        if (heldItem != null)
        {
            heldItem.transform.position = transform.position + transform.forward * (itemRadius) * 2;
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
    /// Handles the invisble buttone being pressed
    /// </summary>
    /// <param name="context"></param>
    private void Invisible(InputAction.CallbackContext context)
    {
        if (invisibilityCooldown == 0f && invisTime == 0f)
        {
            InvisInUse = true;
            // Cast a ray downwards from the player's position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                // Check if the hit object has a renderer
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Get the material of the object below the player
                    Material materialBelowPlayer = renderer.material;

                    // Change the player's material to the materialBelowPlayer
                    ChangePlayerMaterial(materialBelowPlayer);
                }
                gameObject.tag = "Invisible";
            }
        }
        else if (invisTime != 0f)
        {
            TurnOffInvisibility();
        }
    }

    /// <summary>
    /// Updates invisibility variables and cooldowns after activation
    /// </summary>
    private void UpdateInvisibility()
    {
        if (InvisInUse)
        {
            invisTime += Time.deltaTime;
        }
        if (invisTime >= maxInvisTime)
        {
            TurnOffInvisibility();
        }
        if (startInvisCooldown)
        {
            invisibilityCooldown += Time.deltaTime;
        }
        if (invisibilityCooldown >= maxInvisCooldown)
        {
            startInvisCooldown = false;
            invisibilityCooldown = 0;
        }
    }

    private void TurnOffInvisibility()
    {
        gameObject.tag = "Player";
        InvisInUse = false;
        ChangePlayerMaterial(alienSkin);
        startInvisCooldown = true;
        invisTime = 0;
    }

    private void ChangePlayerMaterial(Material newMaterial)
    {
        // Assuming your player has a Renderer component
        Renderer playerRenderer = GetComponent<Renderer>();

        // Change the player's material
        playerRenderer.material = newMaterial;
    }

    private void ReleaseItem()
    {
        if (heldItem.name.Contains("Cow"))
        {
            Cow cow = heldItem.GetComponent<Cow>();
            heldItem.GetComponent<NavMeshAgent>().enabled = true;
            heldItem.GetComponent<NavMeshAgent>().SetDestination(heldItem.transform.position);
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
            moveSpeed = 10f;
        }
        heldItem.tag = "Item";
        nearestItem = null;
        heldItem.transform.parent = null;
        heldItem.transform.position = transform.position + transform.forward * itemRadius * 2;
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

    private void GrabItem()
    {
        // Parent the new nearest item to the player
        nearestItem.transform.parent = gameObject.transform;
        heldItem = nearestItem.gameObject;
        heldItem.tag = "HeldItem";

        //Sets the item radius
        if (heldItem.GetComponent<Item>().sphereCollider != null)
        {
            itemRadius = heldItem.GetComponent<Item>().sphereCollider.radius;
        }
        else if (heldItem.GetComponent<Item>().capsuleCollider != null)
        {
            itemRadius = heldItem.GetComponent<Item>().capsuleCollider.radius;
        }

        //Changes variables for if the item is a cow
        if (heldItem.name.Contains("Cow"))
        {
            heldItem.GetComponent<NavMeshAgent>().enabled = false;
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            moveSpeed = cowSlowDown;
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
