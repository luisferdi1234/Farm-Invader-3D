using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //SerializedFields
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;

    //Item variables
    public GameObject nearestItem;
    private GameObject heldItem;

    //Cinemachine
    private CinemachineVirtualCamera vcam;

    //InputSystem Variables
    public PlayerControls playerControls;
    Vector3 moveDirection = Vector3.zero;
    private InputAction move;
    private InputAction fire;
    private bool FirePressed;

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
        Debug.Log(heldItem);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (nearestItem != null)
        {
            if (heldItem == nearestItem.gameObject)
            {
                // If the nearest item is the same as the held item, unparent it
                heldItem.tag = "Item";
                heldItem.transform.parent = null;
                heldItem.GetComponent<SphereCollider>().enabled = true;
                heldItem = null;
            }
            else if (heldItem == null)
            {
                // Parent the new nearest item to the player
                nearestItem.transform.parent = gameObject.transform;
                heldItem = nearestItem.gameObject;
                heldItem.tag = "HeldItem";
                nearestItem.GetComponent<SphereCollider>().enabled = false;
            }
        }
        else if (heldItem != null)
        {
            heldItem.tag = "Item";
            nearestItem = null;
            heldItem.transform.parent = null;
            heldItem.GetComponent<SphereCollider>().enabled = true;
            heldItem = null;
        }
    }
}
