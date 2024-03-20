using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlienClone : MonoBehaviour
{
    [SerializeField] float maxLifeTime = 5f;
    private float lifeTimer;
    public Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 0;
    Rigidbody rb;
    Animator animator;

    PlayerController controller;
    PlayerControls playerControls;
    CinemachineVirtualCamera vcam;

    InputAction move;
    InputAction useAbility;

    private void Awake()
    {
        controller = GameObject.Find("Alien").GetComponent<PlayerController>();
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = gameObject.transform;
        vcam.LookAt = gameObject.transform;

        controller.canMove = false;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        useAbility = playerControls.Player.UseAbility;
        useAbility.Enable();
    }

    private void OnDisable()
    {
        move.Disable();

        useAbility.Disable();
    }
    private void Update()
    {
        if (lifeTimer <= maxLifeTime)
        {
            lifeTimer += Time.deltaTime;
        }
        else
        {
            DestroyClone();
        }
        moveDirection = move.ReadValue<Vector2>();
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

    public void DestroyClone()
    {
        vcam.LookAt = controller.gameObject.transform;
        vcam.Follow = controller.gameObject.transform;
        controller.canMove = true;
        Destroy(gameObject);
    }

    /// <summary>
    /// Plays Grass Sound using animation event
    /// </summary>
    public void PlayGrassSound()
    {
        AudioManager.instance.PlayRandomAudioClip("grassSounds");
    }
}
