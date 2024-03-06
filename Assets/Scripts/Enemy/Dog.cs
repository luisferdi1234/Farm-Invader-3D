using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy
{
    private int previousSound;

    private float idleTimer = 0f;

    private float searchTimer = 0f;

    [SerializeField] float maxSearchTime = 3f;

    private bool inChase = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine = new EnemyStateMachine(new DogPatrolState(), gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (stateMachine.GetCurrentState().GetType() == typeof(DogPatrolState) && inChase)
        {
            if (agent.velocity.magnitude < 1)
            {
                searchTimer += Time.deltaTime;
                if (searchTimer >= maxSearchTime)
                {
                    stateMachine.ChangeState(new ReturnState(), gameObject);
                    searchTimer = 0f;
                    inChase = false;
                }
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
            stateMachine.ChangeState(new DogPatrolState(), gameObject);
            idleTimer = 0f;
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(IdleState))
        {
            idleTimer += Time.deltaTime;
        }
    }
    void OnTriggerStay(Collider other)
    {
        //Changes to Chase State after spotting player
        if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            stateMachine.ChangeState(new ChaseState(), gameObject);
            inChase = true;
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        //Patrols area after losing sight of player
        else if (other.name.Contains("Alien") && stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && other.CompareTag("Invisible"))
        {
            stateMachine.ChangeState(new ReturnState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
        }
    }
}
