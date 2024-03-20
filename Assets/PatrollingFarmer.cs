using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingFarmer : PatrollingEnemy
{
    //Serialized Fields
    [SerializeField] float detectionAngle = 50f;
    [SerializeField] float maxRayDistance = 20f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float differentRotation = 0f;
    
    //Timers

    private float searchTimer = 0f;

    [SerializeField] float maxSearchTime = 3f;

    private bool inChase = false;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetNextPosition();
        stateMachine = new EnemyStateMachine(new PatrolState(), gameObject);
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
                    stateMachine.ChangeState(new PatrolState(), gameObject);
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
        else if (stateMachine.GetCurrentState().GetType() == typeof(PatrolState))
        {
            float positionThreshold = .5f;
            animator.SetBool("Chasing", false);
            animator.SetBool("Patrolling", false);
            animator.SetBool("Returning", true);
            if (Vector3.Distance(transform.position, nextPosition) <= positionThreshold)
            {
                SetNextPosition();
            }
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
    }

    /// <summary>
    /// Checks collision with clone
    /// </summary>
    /// <param name="other"></param>
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
