using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    private GameObject currentObject;
    private Enemy currentEnemy;
    public void OnEnter(GameObject gameObject)
    {
        currentObject = gameObject;
        currentEnemy = gameObject.GetComponent<Enemy>();
    }

    public void OnUpdate()
    {
        if (currentObject.transform.localRotation != currentEnemy.GetSpawnOrientation())
        {
            currentEnemy.RotateTowardsSpawnOrientation(5f);
        }
    }

    public void OnExit()
    {

    }
}
