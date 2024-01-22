using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReturnState : EnemyState
{
    private GameObject currentObject;
    Enemy currentEnemy;
    NavMeshAgent agent;
    public void OnEnter(GameObject gameObject)
    {
        currentObject = gameObject;
        currentEnemy = gameObject.GetComponent<Enemy>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.SetDestination(currentEnemy.GetSpawnPoint());
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        currentEnemy.RotateTowardsSpawnOrientation(5f);
    }
}
