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
    [SerializeField] Material invisibilityMaterial;
    [SerializeField] GameObject rightHand;
    [SerializeField] GameObject spine;
    [SerializeField] GameObject alienModel;

    //Item variables
    public GameObject nearestItem;
    public GameObject heldItem;
    public float itemRadius;

    //Gadget variables
    [HideInInspector] public float invisTime = 0;
    [HideInInspector] public float invisibilityCooldown = 0f;
    [HideInInspector] public float Energy = 3f;
    public float maxEnergy = 3f;
    private bool InvisInUse = false;
    private bool startInvisCooldown = false;
    private SkinnedMeshRenderer skineedMeshRenderer;


    //Animator
    private Animator animator;

    //Cinemachine
    private CinemachineVirtualCamera vcam;

    //InputSystem Variables
    public PlayerControls playerControls;
    Vector3 moveDirection = Vector3.zero;
    private InputAction move;
    private InputAction fire;
    private InputAction invisible;
    private InputAction recharge;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();

        //Cinemachine
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = gameObject.transform;
        vcam.LookAt = gameObject.transform;

        //Renderer for Invisibility
        skineedMeshRenderer = alienModel.GetComponent<SkinnedMeshRenderer>();

        //Animator
        animator = gameObject.GetComponent<Animator>();
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

        recharge = playerControls.Player.Recharge;
        recharge.Enable();
    }

    private void OnDisable()
    {
        //Input System
        move.Disable();
        fire.Disable();
        invisible.Disable();
        recharge.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        UpdateInvisibility();
        Recharge();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);

        //Sets walk to idle and idle to walk
        animator.SetFloat("Velocity", rb.velocity.magnitude);

        if (moveDirection != Vector3.zero)
        {
            // Calculate rotation to look at the movement direction
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
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
    /// Handles the invisble button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void Invisible(InputAction.CallbackContext context)
    {
        if (invisibilityCooldown == 0f && invisTime == 0f && Energy == maxEnergy)
        {
            InvisInUse = true;
            gameObject.tag = "Invisible";
            AudioManager.instance.PlaySpecificSound("AlienCloak", 1f);
            //Changes Material
            ChangePlayerMaterial(invisibilityMaterial);
        }
    }

    /// <summary>
    /// Refills player energy while near a cow
    /// </summary>
    private void Recharge()
    {
        if (nearestItem != null && nearestItem.name.Contains("Cow") && Energy < maxEnergy && recharge.ReadValue<float>() > 0.5f)
        {
            AudioManager.instance.PlayAlienCharge();
            particleAttractorLinear particles = nearestItem.GetComponent<Item>().lightning.GetComponent<particleAttractorLinear>();
            nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Play();
            particles.target = transform;
            Energy += Time.deltaTime;
            if (Energy > maxEnergy)
            {
                AudioManager.instance.PauseAlienCharge();
                nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
                Energy = maxEnergy;
            }
        }
        else if (nearestItem != null && nearestItem.name.Contains("Cow") && nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().isPlaying)
        {
            AudioManager.instance.PauseAlienCharge();
            nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
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
    }

    /// <summary>
    /// Handles all the variable changes associated with invisibility running out
    /// </summary>
    private void TurnOffInvisibility()
    {
        gameObject.tag = "Player";
        InvisInUse = false;
        ChangePlayerMaterial(alienSkin);
        startInvisCooldown = true;
        invisTime = 0;
        Energy = 0;
    }

    /// <summary>
    /// Changes player material to new material
    /// </summary>
    /// <param name="newMaterial"></param>
    private void ChangePlayerMaterial(Material newMaterial)
    {
        // Change the player's material
        skineedMeshRenderer.material = newMaterial;
    }

    /// <summary>
    /// Lets go of the item you are currently holding
    /// </summary>
    private void ReleaseItem()
    {
        if (heldItem.name.Contains("Cow"))
        {
            Cow cow = heldItem.GetComponent<Cow>();
            heldItem.GetComponent<NavMeshAgent>().enabled = true;
            heldItem.GetComponent<NavMeshAgent>().SetDestination(heldItem.transform.position);
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
            animator.SetBool("CarryingCow", false);
            moveSpeed = 10f;
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
    private void GrabItem()
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
            moveSpeed = cowSlowDown;
            // Set the initial relative rotation of the cow when picked up
            Vector3 relativeRotation = new Vector3(0f, transform.eulerAngles.y + 90f, 0f); // Adjust as needed
            heldItem.transform.localRotation = Quaternion.Euler(relativeRotation);
            heldItem.transform.parent = spine.transform;
            AudioManager.instance.PlayRandomAudioClip("cowSounds", 1f);
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
