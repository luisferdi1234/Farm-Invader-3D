using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneItem : Item
{
    [SerializeField] GameObject clone;
    GameObject player;
    PlayerController playerController;


    private void Awake()
    {
        player = GameObject.Find("Alien");
        playerController = player.GetComponent<PlayerController>();

    }

    public override void UseAbility()
    {
        GameObject currentClone = Instantiate(clone, player.transform.position, player.transform.rotation);
        currentClone.GetComponent<AlienClone>().moveDirection = player.transform.forward;
        currentClone.GetComponent <AlienClone>().moveSpeed = playerController.moveSpeed;
        AudioManager.instance.PlaySpecificSound("Bush Placed", .3f);
        energy = 0f;
    }

    public virtual void TurnOffAbility()
    {
        //Enter turn off code here
    }
}
