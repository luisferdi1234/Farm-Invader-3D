using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy
{
    Animator dogAnimator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dogAnimator = GetComponent<Animator>();
        stateMachine = new EnemyStateMachine(new DogGuardState(), gameObject);
    }

    protected override void Update()
    {
        base.Update();

        if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && target == null)
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            dogAnimator.SetBool("Chasing", false);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
        }
        else if (stateMachine.GetCurrentState().GetType() == typeof(ChaseState) && player.CompareTag("Invisible"))
        {
            stateMachine.ChangeState(new DogGuardState(), gameObject);
            dogAnimator.SetBool("Chasing", false);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
        }
    }
    void OnTriggerStay(Collider other)
    {
        //Changes to Chase State after spotting clone
        if (other.CompareTag("Clone") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            agent.enabled = true;
            stateMachine.ChangeState(new ChaseState(), gameObject);
            dogAnimator.SetBool("Chasing", true);
            AudioManager.instance.PlayRandomAudioClip("dogGrowlSounds");
        }
        //Changes to Chase State after spotting player
        else if (other.CompareTag("Player") && stateMachine.GetCurrentState().GetType() != typeof(ChaseState))
        {
            target = other.gameObject;
            agent.enabled = true;
            stateMachine.ChangeState(new ChaseState(), gameObject);
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
            dogAnimator.SetBool("Chasing", false);
            AudioManager.instance.PlayRandomAudioClip("dogBarkSounds");
            collision.gameObject.GetComponent<AlienClone>().DestroyClone();
        }
    }
}
