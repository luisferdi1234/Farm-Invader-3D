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
    InputAction pause;

    private void Awake()
    {
        //Input System
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (PlayerProgressManager.Instance.playerPosition != Vector3.zero)
        {
            transform.position = PlayerProgressManager.Instance.playerPosition;
        }
        if(PlayerProgressManager.Instance.menuMusic != null && !PlayerProgressManager.Instance.menuMusic.isPlaying)
        {
            PlayerProgressManager.Instance.menuMusic.Play();
        }
    }

    private void OnEnable()
    {
        //Input System
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        pause = playerControls.Player.Pause;
        pause.Enable();
        pause.performed += Pause;
    }

    private void OnDisable()
    {
        //Input System
        move.Disable();

        fire.Disable();

        pause.Disable();
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
        if (currentLevel == "Reset Data")
        {
            PlayerPrefs.DeleteAll();
            DataDeletionManager.instance.DeleteAllOtherLevels();
        }
        else if (currentLevel != "")
        {
            PlayerProgressManager.Instance.SaveLevelSelectPosition(gameObject);
            PlayerProgressManager.Instance.menuMusic.Stop();
            SceneManager.LoadScene(currentLevel);
        }
    }

    /// <summary>
    /// Returns to main menu
    /// </summary>
    /// <param name="context"></param>
    private void Pause(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Main Menu");
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
