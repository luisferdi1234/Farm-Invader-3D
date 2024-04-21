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
    protected float agentRotationSpeed = 0f;

    [HideInInspector] public Animator animator;
    [HideInInspector] public bool hasVision;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public float normalAgentSpeed = 0f;

    protected EnemyStateMachine stateMachine;

    protected virtual void Start()
    {
        player = GameObject.Find("Alien").gameObject;
        agent = GetComponent<NavMeshAgent>();
        agentRotationSpeed = agent.angularSpeed;

        normalAgentSpeed = agent.speed;
        rb = GetComponent<Rigidbody>();
        spawnPoint = gameObject.transform.position;
        spawnOrientation = transform.rotation;
        maxRotation = startingRotation + rotationRange;
        minRotation = startingRotation - rotationRange;
        animator = GetComponent<Animator>();
        hasVision = true;
        if (rotationSpeed < 0)
        {
            rotationSpeed *= -1;
            direction *= -1;
        }
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.UpdateState();

        if (agent.enabled)
        {
            transform.position += agent.velocity * Time.deltaTime;
            // Calculate the rotation only if the agent has a non-zero desired velocity
            if (agent.velocity != Vector3.zero)
            {
                // Smoothly rotate towards the desired velocity using the agent's angular speed
                Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agentRotationSpeed * Time.deltaTime);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
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
