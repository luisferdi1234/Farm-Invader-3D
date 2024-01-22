using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConroller : MonoBehaviour
{
    //SerializedFields
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] SphereCollider sphereCollider;

    private Transform carriedItem;

    //Cinemachine
    private CinemachineVirtualCamera vcam;

    //InputSystem Variables
    public PlayerControls playerControls;
    Vector3 moveDirection = Vector3.zero;
    private InputAction move;
    private InputAction fire;

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
        fire.started += ctx => Fire();
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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        fire.performed.Invoke();
    }
}
