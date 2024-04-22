using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGuardState : EnemyState
{
    private Enemy currentEnemy;
    float idleTimer = 0;
    float maxIdleTime = .1f;
    bool positionReset = false;

    public void OnEnter(GameObject gameObject)
    {
        currentEnemy = gameObject.GetComponent<Enemy>();

        idleTimer = 0;
        positionReset = false;
    }

    public void OnUpdate()
    {
        if (idleTimer >= maxIdleTime && positionReset == false)
        {
            currentEnemy.agent.isStopped = true;
            currentEnemy.agent.updatePosition = true;
            currentEnemy.agent.updateRotation = true;
            currentEnemy.agent.enabled = false;
            positionReset = true;
        }
        else if(currentEnemy.agent.velocity.magnitude <= 1f)
        {
            idleTimer += Time.deltaTime;
        }
    }

    public void OnExit()
    {
        currentEnemy.agent.updatePosition = false;
        currentEnemy.agent.updateRotation = false;
    }
}
