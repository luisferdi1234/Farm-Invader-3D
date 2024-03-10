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

        if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && player.CompareTag("Invisible"))
        {
            stateMachine.ChangeState(new DogPatrolState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
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
    }
}
