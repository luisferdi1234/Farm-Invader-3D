using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public SphereCollider sphereCollider;
    [SerializeField] public CapsuleCollider capusleCollider;
    [SerializeField] public GameObject lightning;
    [SerializeField] public bool isRechargeable;
    [SerializeField] public bool hasAbility = false;
    [SerializeField] public bool stackable = false;
    [SerializeField] public bool isReusable = true;
    [SerializeField] public int numberOfUses = -1;
    [SerializeField] public string itemName;
    [SerializeField] public float maxEnergy = 3f;
    public float energy = 3f;

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
