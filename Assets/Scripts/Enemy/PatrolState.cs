using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Makes enemy head swivel while standing still guaridng an area
/// </summary>
public class PatrolState : EnemyState
{
    private GameObject currentObject;
    private Enemy currentEnemy;
    private Farmer currentFarmer;
    private GameObject spine;
    float rotation;
    private float currentRotation = 0f;
    public void OnEnter(GameObject gameObject)
    {
        currentObject = gameObject;
        currentEnemy = gameObject.GetComponent<Enemy>();
        currentFarmer = gameObject.GetComponent<Farmer>();
        spine = currentFarmer.spine;
        currentRotation = currentEnemy.startingRotation;
    }

    public void OnUpdate()
    {
        rotation = currentEnemy.rotationSpeed * currentEnemy.direction * Time.deltaTime;
        currentObject.transform.Rotate(Vector3.up * rotation);
        currentRotation += rotation;
        // Check if rotation exceeds the range, change direction if needed
        if (currentRotation >= currentEnemy.maxRotation || currentRotation <= currentEnemy.minRotation)
        {
            currentEnemy.direction *= -1; // Change direction
        }
    }

    public void OnExit()
    {

    }
}
