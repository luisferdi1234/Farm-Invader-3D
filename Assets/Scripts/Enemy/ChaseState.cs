using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private GameObject currentObject;
    Enemy currentEnemy;
    NavMeshAgent agent;
    GameObject player;
    public void OnEnter(GameObject gameObject)
    {
        currentEnemy = gameObject.GetComponent<Enemy>();
        currentObject = gameObject;
        agent = currentObject.GetComponent<NavMeshAgent>();
        player = currentEnemy.GetPlayer();
    }

    public void OnUpdate()
    {
        agent.SetDestination(player.transform.position);
    }

    public void OnExit()
    {

    }
}
