using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UFOController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    Vector3 moveDirection = Vector3.zero;
    Rigidbody rb;

    [HideInInspector] public GameObject levelObject;

    [HideInInspector] public string currentLevel = null;

    //Player controls
    PlayerControls playerControls;

    InputAction move;
    InputAction fire;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
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

    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.y * moveSpeed);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (currentLevel != "")
        {
            SceneManager.LoadScene(currentLevel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Level"))
        {
            levelObject = other.gameObject;
            levelObject.GetComponent<Outline>().enabled = true;
            currentLevel = levelObject.GetComponent<LevelInfo>().levelName;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Level"))
        {
            levelObject.GetComponent<Outline>().enabled = false;
            currentLevel = null;
            levelObject = null;
        }
    }
}
