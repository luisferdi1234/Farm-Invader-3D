using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Farmer : Enemy
{
    //Serialized Fields
    [SerializeField] float detectionAngle = 28f;
    [SerializeField] float maxRayDistance = 30f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float differentRotation = 0f;

    //Timers

    private float idleTimer = 0f;

    private float searchTimer = 0f;

    [SerializeField] float maxSearchTime = 3f;

    private bool inChase = false;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine = new EnemyStateMachine(new SearchState(), gameObject);
        if (differentRotation != startingRotation)
        {
            startingRotation = differentRotation;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetFloat("Velocity", agent.velocity.magnitude);

        if (stateMachine.GetCurrentState().GetType() == typeof(SearchState) && inChase)
        {
            if (agent.velocity.magnitude < 1)
            {
                searchTimer += Time.deltaTime;
                if (searchTimer >= maxSearchTime)
                {
                    animator.enabled = true;
                    animator.SetBool("Chasing", false);
                    animator.SetBool("Patrolling", false);
                    animator.SetBool("Returning", true);
                    stateMachine.ChangeState(new ReturnState(), gameObject);
                    searchTimer = 0f;
                    inChase = false;
                }
            }
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && target == null)
        {
            animator.enabled = true;
            animator.SetBool("Chasing", false);
            animator.SetBool("Patrolling", true);
            animator.SetBool("Returning", false);

            stateMachine.ChangeState(new SearchState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("returnSounds");
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState))
        {
            //Patrols area after losing player
            if (player.CompareTag("Invisible") || !hasVision)
            {
                animator.enabled = true;
                animator.SetBool("Chasing", false);
                animator.SetBool("Patrolling", true);
                animator.SetBool("Returning", false);

                stateMachine.ChangeState(new SearchState(), gameObject);
                AudioManager.instance.PlayRandomAudioClip("returnSounds");
            }
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ReturnState))
        {
            float positionThreshold = 0.5f;
            if (Vector3.Distance(gameObject.transform.position, base.GetSpawnPoint()) <= positionThreshold)
            {
                stateMachine.ChangeState(new IdleState(), gameObject);
            }
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(IdleState) && idleTimer >= 1f)
        {
            animator.SetBool("Chasing", false);
            animator.SetBool("Returning", false);
            animator.SetBool("Patrolling", true);
            stateMachine.ChangeState(new SearchState(), gameObject);
            idleTimer = 0f;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(IdleState))
        {
            animator.SetBool("Chasing", false);
            animator.SetBool("Returning", false);
            animator.SetBool("Patrolling", true);
            idleTimer += Time.deltaTime;
        }
    }
    void OnTriggerStay(Collider other)
    {
        //Changes to Chase State after spotting player
        if (other.CompareTag("Clone") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            if (hasVision && CheckLineOfSight(other.gameObject))
            {
                animator.enabled = true;
                animator.SetBool("Chasing", true);
                animator.SetBool("Patrolling", false);
                animator.SetBool("Returning", false);
                target = other.gameObject;
                stateMachine.ChangeState(new ChaseState(), gameObject);
                AudioManager.instance.PlayRandomAudioClip("chaseSounds");
                inChase = true;
            }
        }
        //Changes to Chase State after spotting player
        else if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            if (hasVision && CheckLineOfSight(other.gameObject))
            {
                animator.enabled = true;
                animator.SetBool("Chasing", true);
                animator.SetBool("Patrolling", false);
                animator.SetBool("Returning", false);
                target = other.gameObject;
                stateMachine.ChangeState(new ChaseState(), gameObject);
                ScoreManager.Instance.SpottedAlien();
                AudioManager.instance.PlayRandomAudioClip("chaseSounds");
                inChase = true;
            }
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && other.CompareTag("Clone") && target != other.gameObject)
        {
            target = other.gameObject;
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.gameObject.tag == "Clone")
        {
            other.gameObject.GetComponent<AlienClone>().DestroyClone();
            animator.enabled = true;
            animator.SetBool("Chasing", false);
            animator.SetBool("Patrolling", true);
            animator.SetBool("Returning", false);

            stateMachine.ChangeState(new SearchState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("returnSounds");
        }
    }

    /// <summary>
    /// Checks to see if the player is within the line of sight of the farmer
    /// </summary>
    /// <returns></returns>
    bool CheckLineOfSight(GameObject other)
    {
        // Calculate the vector to the player
        Vector3 toTarget = other.transform.position - transform.position;

        // Calculate the angle between the enemy's forward vector and the vector to the player
        float angle = Vector3.Angle(spine.transform.up, toTarget);

        // Check if the angle is within the allowed range
        if (angle <= detectionAngle)
        {
            // Shoot a ray to check for obstacles
            RaycastHit hit;

            if (Physics.Raycast(transform.position, toTarget.normalized, out hit, maxRayDistance, obstacleMask) && (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "Clone"))
            {
                // Obstacle is hit, line of sight is blocked
                return true;
            }
            else
            {
                // No obstacles, player is in line of sight
                return false;
            }
        }
        else
        {
            Debug.Log("Player not in range");
            return false;
        }
    }

    /// <summary>
    /// Plays Grass Sound using animation event
    /// </summary>
    private void PlayGrassSound()
    {
        AudioManager.instance.PlayRandomAudioClip("grassSounds");
    }
}
