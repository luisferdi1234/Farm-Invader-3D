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
    [SerializeField] public float maxMoveSpeed = 9f;
    [SerializeField] public float cowSlowDown = 2f;
    [SerializeField] float maxInvisTime = 3f;
    [SerializeField] Material alienSkin;
    [SerializeField] Material invisibilityMaterial;
    [SerializeField] GameObject alienModel;

    [HideInInspector] public float moveSpeed = 9f;

    //Gadget variables
    [HideInInspector] public float invisTime = 0;
    [HideInInspector] public float invisibilityCooldown = 0f;
    [HideInInspector] public float Energy = 3f;
    public float maxEnergy = 3f;
    private bool InvisInUse = false;
    private bool startInvisCooldown = false;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Inventory inventory;


    //Animator
    private Animator animator;

    //Cinemachine
    private CinemachineVirtualCamera vcam;

    //InputSystem Variables
    public PlayerControls playerControls;
    Vector3 moveDirection = Vector3.zero;
    private InputAction move;
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
        skinnedMeshRenderer = alienModel.GetComponent<SkinnedMeshRenderer>();

        //Animator
        animator = gameObject.GetComponent<Animator>();

        //Inventory
        inventory = gameObject.GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        //Input System
        move = playerControls.Player.Move;
        move.Enable();

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
    }

    /// <summary>
    /// Handles the invisble button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void Invisible(InputAction.CallbackContext context)
    {
        if (invisibilityCooldown == 0f && invisTime == 0f && Energy == maxEnergy && (inventory.inventorySlots[inventory.currentInventorySlot, 0] == null || !inventory.inventorySlots[inventory.currentInventorySlot, 0].name.Contains("Cow")))
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
        if (inventory.nearestItem != null && inventory.nearestItem.name.Contains("Cow") && Energy < maxEnergy && recharge.ReadValue<float>() > 0.5f)
        {
            AudioManager.instance.PlayAlienCharge();
            particleAttractorLinear particles = inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<particleAttractorLinear>();
            inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Play();
            particles.target = transform;
            Energy += Time.deltaTime;
            if (Energy > maxEnergy)
            {
                AudioManager.instance.PauseAlienCharge();
                inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
                Energy = maxEnergy;
            }
        }
        else if (inventory.nearestItem != null && inventory.nearestItem.name.Contains("Cow") && inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().isPlaying)
        {
            AudioManager.instance.PauseAlienCharge();
            inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
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
        skinnedMeshRenderer.material = newMaterial;
    }

    /// <summary>
    /// Plays Grass Sound using animation event
    /// </summary>
    private void PlayGrassSound()
    {
        AudioManager.instance.PlayRandomAudioClip("grassSounds");
    }
}
