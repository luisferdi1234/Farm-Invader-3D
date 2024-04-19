using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingDog : PatrollingEnemy
{
    //How long the dog can be lured for before going back to patrolling
    [SerializeField] float maxIdleTime;
    [SerializeField] float maxTurnTime;
    private float idleTimer = 0f;
    private float turnTimer = 0f;
    Animator dogAnimator;

    protected override void Start()
    {
        base.Start();
        SetNextPosition();
        dogAnimator = GetComponent<Animator>();
        dogAnimator.SetBool("Walking", true);
        stateMachine = new EnemyStateMachine(new PatrolState(), gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && target == null)
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            dogAnimator.SetBool("Chasing", false);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            idleTimer = 0f;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && player.CompareTag("Invisible"))
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            dogAnimator.SetBool("Chasing", false);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            idleTimer = 0f;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(DogGuardState) && idleTimer < maxIdleTime)
        {
            idleTimer += Time.deltaTime;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(DogGuardState) && idleTimer >= maxIdleTime)
        {
            agent.enabled = true;
            dogAnimator.SetBool("Walking", true);
            stateMachine.ChangeState(new PatrolState(), gameObject);
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(PatrolState))
        {
            float positionThreshold = .5f;
            if (Vector3.Distance(transform.position, nextPosition) <= positionThreshold)
            {
                dogAnimator.SetBool("Walking", false);
                SetNextPosition();
            }
            if (turnTimer < maxTurnTime)
            {
                turnTimer += Time.deltaTime;
            }
            else if (turnTimer >= maxTurnTime)
            {
                turnTimer = 0f;
                dogAnimator.SetBool("Walking", true);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Changes to Chase State after spotting clone
        if (other.CompareTag("Clone") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            stateMachine.ChangeState(new ChaseState(), gameObject);
            agent.enabled = true;
            dogAnimator.SetBool("Chasing", true);
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        //Changes to Chase State after spotting player
        else if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            stateMachine.ChangeState(new ChaseState(), gameObject);
            agent.enabled = true;
            dogAnimator.SetBool("Chasing", true);
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && other.CompareTag("Clone") && target != other.gameObject)
        {
            target = other.gameObject;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Inventory>().TurnOffInventoryControls();
            collision.gameObject.GetComponent<PlayerController>().PlayDeathAnimation();
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            dogAnimator.SetBool("Chasing", false);
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        else if (collision.gameObject.tag == "Clone")
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            dogAnimator.SetBool("Chasing", false);
            collision.gameObject.GetComponent<AlienClone>().DestroyClone();
            idleTimer = 0f;
        }
    }
}
