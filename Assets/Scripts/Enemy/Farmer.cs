using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Farmer : Enemy
{
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] float detectionAngle = 50f;
    [SerializeField] float maxRayDistance = 20f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float differentRotation = 0f;
    [SerializeField] public GameObject spine;
    [HideInInspector] public Animator animator;

    private int previousSound;

    private float idleTimer = 0f;

    private float searchTimer = 0f;

    private bool inChase = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (differentRotation != startingRotation)
        {
            startingRotation = differentRotation;
        }
        animator = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Debug.Log(stateMachine.GetCurrentState());

        if (stateMachine.GetCurrentState().GetType() == typeof(PatrolState) && inChase)
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= 3f)
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
            stateMachine.ChangeState(new PatrolState(), gameObject);
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
        if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            if (CheckLineOfSight())
            {
                animator.enabled = true;
                animator.SetBool("Chasing", true);
                animator.SetBool("Patrolling", false);
                animator.SetBool("Returning", false);
                stateMachine.ChangeState(new ChaseState(), gameObject);
                inChase = true;
            }
        }
        //Patrols area after losing sight of player
        else if (other.name.Contains("Alien") && stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && other.CompareTag("Invisible"))
        {
            animator.enabled = true;
            animator.SetBool("Chasing", false);
            animator.SetBool("Patrolling", true);
            animator.SetBool("Returning", false);
            stateMachine.ChangeState(new PatrolState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("returnSounds", .5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Changes to return state after getting out of range from player
        if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(PatrolState))
        {
            stateMachine.ChangeState(new ReturnState(), gameObject);
        }
    }

    /// <summary>
    /// Checks to see if the player is within the line of sight of the farmer
    /// </summary>
    /// <returns></returns>
    bool CheckLineOfSight()
    {
        // Calculate the vector to the player
        Vector3 toPlayer = player.transform.position - transform.position;

        // Calculate the angle between the enemy's forward vector and the vector to the player
        float angle = Vector3.Angle(spine.transform.up, toPlayer);

        // Check if the angle is within the allowed range
        if (angle <= detectionAngle)
        {
            // Shoot a ray to check for obstacles
            RaycastHit hit;

            if (Physics.Raycast(transform.position, toPlayer.normalized, out hit, maxRayDistance, obstacleMask) && hit.collider.gameObject.name == "Alien")
            {
                // Obstacle is hit, line of sight is blocked
                Debug.Log("Player in line of sight");
                return true;
            }
            else
            {
                // No obstacles, player is in line of sight
                Debug.Log("Line of sight blocked by obstacle");
                return false;
            }
        }
        else
        {
            Debug.Log("Player not in range");
            return false;
        }
    }
}
