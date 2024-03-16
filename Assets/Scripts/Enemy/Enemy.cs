using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    protected GameObject player;
    protected Vector3 spawnPoint;
    protected Quaternion spawnOrientation;
    public GameObject target;
    [SerializeField] public GameObject spine;
    public float rotationSpeed = 60f;
    public float rotationRange = 90f;
    public int direction = 1; //1 for clockwise, -1 for counter-clockwise
    public float startingRotation = 0f;
    public float maxRotation;
    public float minRotation;

    [HideInInspector] public Animator animator;
    [HideInInspector] public bool hasVision;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody rb;

    protected EnemyStateMachine stateMachine;

    protected virtual void Start()
    {
        player = GameObject.Find("Alien").gameObject;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        spawnPoint = gameObject.transform.position;
        spawnOrientation = transform.rotation;
        maxRotation = startingRotation + rotationRange;
        minRotation = startingRotation - rotationRange;
        animator = GetComponent<Animator>();
        hasVision = true;
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.UpdateState();
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }

    public Quaternion GetSpawnOrientation()
    {
        return spawnOrientation;
    }

    public void RotateTowardsSpawnOrientation(float rotationSpeed)
    {
        // Smoothly rotate using Quaternion.Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, spawnOrientation, rotationSpeed * Time.deltaTime);
    }

    public GameObject GetCurrentEnemy()
    {
        return gameObject;
    }
}
