using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private GameObject currentObject;
    Enemy currentEnemy;
    NavMeshAgent agent;
    int frameCounter = 0;
    public void OnEnter(GameObject gameObject)
    {
        currentEnemy = gameObject.GetComponent<Enemy>();
        currentObject = gameObject;
        agent = currentObject.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;
        agent.speed = currentEnemy.normalAgentSpeed;
    }

    public void OnUpdate()
    {
        if (currentEnemy.target != null)
        {
            agent.SetDestination(currentEnemy.target.transform.position);
        }
        else
        {
            agent.SetDestination(currentObject.transform.position);
        }

    }

    public void OnExit()
    {

    }
}
