using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnState : EnemyState
{
    private GameObject currentObject;
    Enemy currentEnemy;
    NavMeshAgent agent;
    bool connectTransform;
    private float connectTransformTimer = 0f;
    public void OnEnter(GameObject gameObject)
    {
        currentObject = gameObject;
        currentEnemy = gameObject.GetComponent<Enemy>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        currentEnemy.agent.updatePosition = true;
        currentEnemy.agent.updateRotation = true;
        connectTransform = false;

        agent.enabled = true;
        agent.SetDestination(currentEnemy.GetSpawnPoint());
        agent.speed = 5;
    }

    public void OnUpdate()
    {
        if (connectTransformTimer >= 0.1f && connectTransform == false)
        {
            currentEnemy.agent.updatePosition = false;
            currentEnemy.agent.updateRotation = false;
            connectTransform = true;
            connectTransformTimer = 0f;
        }
        else if (connectTransform == false)
        {
            connectTransformTimer += Time.deltaTime;
        }
    }

    public void OnExit()
    {
        currentEnemy.RotateTowardsSpawnOrientation(5f);
    }
}
