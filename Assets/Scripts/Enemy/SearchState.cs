using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Makes enemy head swivel while standing still guaridng an area
/// </summary>
public class SearchState : EnemyState
{
    private GameObject currentObject;
    private Enemy currentEnemy;
    private GameObject spine;
    float rotation;
    private float currentRotation = 0f;
    private float turnOffTimer = 0f;
    private float connectTransformTimer = 0f;
    private bool connectTransform;
    public void OnEnter(GameObject gameObject)
    {
        currentObject = gameObject;
        currentEnemy = gameObject.GetComponent<Enemy>();
        spine = currentEnemy.spine;
        currentRotation = currentEnemy.startingRotation;
        currentEnemy.agent.updatePosition = true;
        currentEnemy.agent.updateRotation = true;
        connectTransform = false;
    }

    public void OnUpdate()
    {
        if (turnOffTimer >= .2f && currentEnemy.hasVision)
        {
            if (currentEnemy.animator.enabled)
            {
                currentEnemy.animator.enabled = false;
            }
            if (currentEnemy.agent.velocity.magnitude <= 1f)
            {
                currentEnemy.agent.enabled = false;
            }
            rotation = currentEnemy.rotationSpeed * currentEnemy.direction * Time.deltaTime;
            spine.transform.Rotate(Vector3.right * rotation);
            currentRotation += rotation;
            // Check if rotation exceeds the range, change direction if needed
            if (currentRotation >= currentEnemy.maxRotation)
            {
                currentEnemy.direction = -1; // Change direction
            }
            else if (currentRotation <= currentEnemy.minRotation)
            {
                currentEnemy.direction = 1;
            }
        }
        else if (currentEnemy.agent.velocity.magnitude < 1)
        {
            turnOffTimer += Time.deltaTime;
        }

        if (connectTransformTimer >= 0.1f && connectTransform == false)
        {
            currentEnemy.agent.updatePosition = false;
            currentEnemy.agent.updateRotation = false;
            connectTransform = true;
        }
        else if (connectTransform == false)
        {
            connectTransformTimer += Time.deltaTime;
        }
    }

    public void OnExit()
    {
        turnOffTimer = 0f;
    }
}
