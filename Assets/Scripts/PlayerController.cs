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
    [SerializeField] GameObject alienModel;

    [HideInInspector] public float moveSpeed = 9f;

    //Gadget variables
    private Inventory inventory;
    private Item currentItem;

    //Animator
    private Animator animator;

    //Cinemachine
    private CinemachineVirtualCamera vcam;

    //InputSystem Variables
    public PlayerControls playerControls;
    Vector3 moveDirection = Vector3.zero;
    private InputAction move;
    private InputAction useAbility;
    private InputAction recharge;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();

        //Cinemachine
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = gameObject.transform;
        vcam.LookAt = gameObject.transform;

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

        useAbility = playerControls.Player.UseAbility;
        useAbility.Enable();
        useAbility.performed += UseAbility;

        recharge = playerControls.Player.Recharge;
        recharge.Enable();
    }

    private void OnDisable()
    {
        //Input System
        move.Disable();
        useAbility.Disable();
        recharge.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.inventorySlots[inventory.currentInventorySlot, 0] == null)
        {
            currentItem = null;
        }
        else
        {
            currentItem = inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>();
        }
        moveDirection = move.ReadValue<Vector2>();
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
    private void UseAbility(InputAction.CallbackContext context)
    {
        if (currentItem != null && currentItem.hasAbility)
        {
            inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>().UseAbility();
        }
    }

    /// <summary>
    /// Refills player energy while near a cow
    /// </summary>
    private void Recharge()
    {
        if (recharge.ReadValue<float>() > 0.5f && inventory.nearestItem != null && inventory.nearestItem.name.Contains("Cow") && currentItem != null && currentItem.isRechargeable && currentItem.energy < currentItem.maxEnergy)
        {
            AudioManager.instance.PlayAlienCharge();
            particleAttractorLinear particles = inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<particleAttractorLinear>();
            inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Play();
            particles.target = transform;
            currentItem.energy += Time.deltaTime;
            if (currentItem.energy > currentItem.maxEnergy)
            {
                AudioManager.instance.PauseAlienCharge();
                inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
                currentItem.energy = currentItem.maxEnergy;
            }
        }
        else if (inventory.nearestItem != null && inventory.nearestItem.name.Contains("Cow") && inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().isPlaying)
        {
            AudioManager.instance.PauseAlienCharge();
            inventory.nearestItem.GetComponent<Item>().lightning.GetComponent<ParticleSystem>().Stop();
        }
    }

    /// <summary>
    /// Plays Grass Sound using animation event
    /// </summary>
    private void PlayGrassSound()
    {
        AudioManager.instance.PlayRandomAudioClip("grassSounds");
    }
}
