using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private GameObject currentObject;
    PatrollingEnemy currentEnemy;
    NavMeshAgent agent;
    Vector3 currentPosition;
    public void OnEnter(GameObject gameObject)
    {
        currentEnemy = gameObject.GetComponent<PatrollingEnemy>();
        currentObject = gameObject;
        agent = currentObject.GetComponent<NavMeshAgent>();
        agent.speed = 8;
        agent.SetDestination(currentEnemy.nextPosition);
    }

    public void OnUpdate()
    {
        if (currentPosition != currentEnemy.nextPosition)
        {
            currentPosition = currentEnemy.nextPosition;
            agent.SetDestination(currentPosition);
        }
    }

    public void OnExit()
    {

    }
}
