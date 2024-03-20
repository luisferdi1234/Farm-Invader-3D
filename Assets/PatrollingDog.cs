using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingDog : PatrollingEnemy
{
    //How long the dog can be lured for before going back to patrolling
    [SerializeField] float maxIdleTime;
    private float idleTimer = 0f;

    protected override void Start()
    {
        base.Start();
        SetNextPosition();
        stateMachine = new EnemyStateMachine(new PatrolState(), gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && target == null)
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            idleTimer = 0f;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && player.CompareTag("Invisible"))
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            idleTimer = 0f;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(DogGuardState) && idleTimer < maxIdleTime)
        {
            idleTimer += Time.deltaTime;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(DogGuardState) && idleTimer >= maxIdleTime)
        {
            stateMachine.ChangeState(new PatrolState(), gameObject);
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(PatrolState))
        {
            float positionThreshold = .5f;
            if (Vector3.Distance(transform.position, nextPosition) <= positionThreshold)
            {
                SetNextPosition();
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
            rb.mass = 10;
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        //Changes to Chase State after spotting player
        else if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            stateMachine.ChangeState(new ChaseState(), gameObject);
            rb.mass = 10;
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag == "Clone")
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            collision.gameObject.GetComponent<AlienClone>().DestroyClone();
            idleTimer = 0f;
        }
    }
}
