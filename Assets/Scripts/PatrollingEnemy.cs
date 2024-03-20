using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    [SerializeField] List<GameObject> positions = new List<GameObject>();

    [HideInInspector] public Vector3 nextPosition = Vector3.zero;
    int currentListPosition = 0;

    /// <summary>
    /// Makes the patrolling enemy start walking to the next position in the list
    /// </summary>
    protected void SetNextPosition()
    {
        currentListPosition++;
        if (currentListPosition >= positions.Count)
        {
            currentListPosition = 0;
        }
        nextPosition = positions[currentListPosition].transform.position;
    }
}
