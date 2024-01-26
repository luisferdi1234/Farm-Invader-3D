using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Makes enemy head swivel while standing still guaridng an area
/// </summary>
public class PatrolState : EnemyState
{
    private GameObject currentObject;
    private Enemy currentEnemy;
    float rotation;
    private float currentRotation = 0f;
    public void OnEnter(GameObject gameObject)
    {
        currentObject = gameObject;
        currentEnemy = gameObject.GetComponent<Enemy>();
        currentRotation = currentEnemy.startingRotation;
    }

    public void OnUpdate()
    {
        rotation = currentEnemy.rotationSpeed * Time.deltaTime;
        currentObject.transform.Rotate(Vector3.up * rotation);

        currentRotation += rotation;
        // Check if rotation exceeds the range, change direction if needed
        if (Mathf.Abs(currentRotation) >= currentEnemy.rotationRange)
        {
            currentEnemy.rotationSpeed *= -1; // Change direction
        }
    }

    public void OnExit()
    {

    }
}
