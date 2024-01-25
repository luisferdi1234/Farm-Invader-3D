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

    //Item variables
    public GameObject nearestItem;
    private GameObject heldItem;
    public float itemRadius;

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
        // Calculate rotation to look at the movement direction
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        if (heldItem != null)
        {
            heldItem.transform.position = transform.position + transform.forward * itemRadius;
        }
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
                if (heldItem.name == "Cow")
                {
                    heldItem.GetComponent<NavMeshAgent>().enabled = true;
                    heldItem.GetComponent<Rigidbody>().isKinematic = true;
                    moveSpeed = 10f;
                }
                // If the nearest item is the same as the held item, unparent it
                heldItem.tag = "Item";
                heldItem.transform.parent = null;
                heldItem.transform.position = transform.position + transform.forward * itemRadius;
                nearestItem = null;

                //Get all colliders attached to the
                Collider[] allColliders = heldItem.GetComponents<Collider>();
                // Disable each collider
                foreach (Collider collider in allColliders)
                {
                    collider.enabled = true;
                }
                heldItem = null;
            }
            else if (heldItem == null)
            {
                // Parent the new nearest item to the player
                nearestItem.transform.parent = gameObject.transform;
                heldItem = nearestItem.gameObject;
                heldItem.tag = "HeldItem";
                if (heldItem.name == "Cow")
                {
                    heldItem.GetComponent<NavMeshAgent>().enabled = false;
                    heldItem.GetComponent<Rigidbody>().isKinematic = false;
                    moveSpeed = 5f;
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
        else if (heldItem != null)
        {
            if (heldItem.name == "Cow")
            {
                heldItem.GetComponent<NavMeshAgent>().enabled = true;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                moveSpeed = 10f;
            }
            heldItem.tag = "Item";
            nearestItem = null;
            heldItem.transform.parent = null;
            heldItem.transform.position = transform.position + transform.forward * itemRadius;
            //Get all colliders attached to the
            Collider[] allColliders = heldItem.GetComponents<Collider>();
            // Disable each collider
            foreach (Collider collider in allColliders)
            {
                collider.enabled = true;
            }
            heldItem = null;
            nearestItem = null;
        }
    }
}
