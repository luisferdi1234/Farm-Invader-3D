using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    protected Vector3 spawnPoint;
    protected Quaternion spawnOrientation;
    public float rotationSpeed = 30f;
    public float rotationRange = 70;
    public int direction = 1; //1 for clockwise, -1 for counter-clockwise

    protected EnemyStateMachine stateMachine;

    protected virtual void Start()
    {
        player = GameObject.Find("Alien").gameObject;
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyStateMachine(new PatrolState(), gameObject);
        spawnPoint = gameObject.transform.position;
        spawnOrientation = transform.rotation;
    }

    protected virtual void Update()
    {
        stateMachine.UpdateState();
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
}
