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
        stateMachine = new EnemyStateMachine(new DogGuardState(), gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && target == null)
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && player.CompareTag("Invisible"))
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
        }
    }
    void OnTriggerStay(Collider other)
    {
        //Changes to Chase State after spotting clone
        if (other.CompareTag("Clone") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            stateMachine.ChangeState(new ChaseState(), gameObject);
            inChase = true;
            rb.mass = 10;
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        //Changes to Chase State after spotting player
        else if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            stateMachine.ChangeState(new ChaseState(), gameObject);
            inChase = true;
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
        }
    }
}
