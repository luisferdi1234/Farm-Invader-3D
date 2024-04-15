using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //SerializedFields
    [SerializeField] public Rigidbody rb;
    [SerializeField] public float maxMoveSpeed = 9f;
    [SerializeField] public float cowSlowDown = 2f;
    [SerializeField] GameObject alienModel;

    public bool inUFO = false;

    [HideInInspector] public float moveSpeed = 9f;

    //Gadget variables
    private Inventory inventory;
    private Item currentItem;
    private float moveTimer = 0;
    private float maxMoveTimer = .20f;
    public bool shouldMove = false;
    public bool canMove = true;

    //Animator
    private Animator animator;

    //InputSystem Variables
    public PlayerControls playerControls;
    [HideInInspector] public Vector3 moveDirection = Vector3.zero;
    private InputAction move;
    private InputAction useAbility;
    private InputAction recharge;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();
        canMove = true;

        //Sets camera to player
        CinemachineVirtualCamera vcam;
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.LookAt = transform;
        vcam.Follow = transform;

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
        if (shouldMove)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= maxMoveTimer)
            {
                shouldMove = false;
                moveTimer = 0;
                canMove = true;
            }
        }
        if (inventory.inventorySlots[inventory.currentInventorySlot, 0] == null)
        {
            currentItem = null;
        }
        else
        {
            currentItem = inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>();
        }
        if (canMove)
        {
            moveDirection = move.ReadValue<Vector2>();
            Recharge();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
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
        else
        {
            rb.velocity = Vector3.zero;

            //Sets walk to idle and idle to walk
            animator.SetFloat("Velocity", rb.velocity.magnitude);
        }
    }

    /// <summary>
    /// Handles the invisble button being pressed
    /// </summary>
    /// <param name="context"></param>
    private void UseAbility(InputAction.CallbackContext context)
    {
        if (canMove && currentItem != null && currentItem.hasAbility && currentItem.energy >= currentItem.maxEnergy)
        {
            inventory.inventorySlots[inventory.currentInventorySlot, 0].GetComponent<Item>().UseAbility();
            if (currentItem.isReusable == false)
            {
                inventory.ReleaseItem();
                Destroy(currentItem.gameObject);
            }
        }
    }

    /// <summary>
    /// Refills player energy while near a cow
    /// </summary>
    private void Recharge()
    {
        if (recharge.ReadValue<float>() > 0.5f && inUFO)
        {
            if (ScoreManager.Instance.LevelCompleted())
            {
                gameObject.SetActive(false);
            }
        }
        else
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
    }

    /// <summary>
    /// Plays Grass Sound using animation event
    /// </summary>
    private void PlayGrassSound()
    {
        AudioManager.instance.PlayRandomAudioClip("grassSounds");
    }

    public void PlayDeathAnimation()
    {
        //Plays a random death animation
        int Selection = Random.Range(1, 3);
        animator.SetInteger("Death", Selection);

        //Plays Sound
        AudioManager.instance.PlayRandomAudioClip("alienDeathSounds");

        //Turns off player collider
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Death";

        //Turn Off Player Controls during death animation
        GetComponent<PlayerInput>().enabled = false;
        move.Disable();
        useAbility.Disable();
        recharge.Disable();
    }

    /// <summary>
    /// Restarts level after animation ends
    /// </summary>
    public void DeathAnimationOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
