using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowItem : Item
{
    [SerializeField] public GameObject itemDetector;
    public virtual void Start()
    {
        //Enter start code here
    }

    public virtual void Update()
    {
        //Enter update code here
    }
    public virtual void UseAbility()
    {
        //Enter ability code
    }

    public virtual void TurnOffAbility()
    {
        //Enter turn off code here
    }
}
