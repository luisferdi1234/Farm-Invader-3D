using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGuardState : EnemyState
{
    private Enemy currentEnemy;

    public void OnEnter(GameObject gameObject)
    {
        currentEnemy = gameObject.GetComponent<Enemy>();
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
}
