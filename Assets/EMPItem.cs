using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPItem : Item
{
    GameObject empManager;
    public override void Start()
    {
        base.Start();
        empManager = GameObject.Find("EMPManager");
    }
    public override void UseAbility()
    {
        empManager.GetComponent<EMPManager>().EMPOn();
        energy = 0f;
    }

    public override void TurnOffAbility()
    {
        //Enter turn off code here
    }
}
