using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGuardState : EnemyState
{
    private Enemy currentEnemy;
    float idleTimer = 0;
    float maxIdleTime = 5f;
    bool positionReset = false;

    public void OnEnter(GameObject gameObject)
    {
        currentEnemy = gameObject.GetComponent<Enemy>();

        idleTimer = 0;
        positionReset = false;
    }

    public void OnUpdate()
    {
        if (idleTimer <= maxIdleTime && positionReset == false)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > maxIdleTime)
            {
                currentEnemy.agent.isStopped = true;
                currentEnemy.agent.enabled = false;
                positionReset = true;
            }
        }
    }

    public void OnExit()
    {
    }
}
