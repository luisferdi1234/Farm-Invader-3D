using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloBushItem : Item
{
    [SerializeField] GameObject bush;
    GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Alien");
    }
    public override void UseAbility()
    {
        Instantiate(bush, player.transform.position + player.transform.forward, player.transform.rotation);
        AudioManager.instance.PlaySpecificSound("Bush Placed", .3f);
        energy = 0f;
    }

    public virtual void TurnOffAbility()
    {
        //Enter turn off code here
    }
}
