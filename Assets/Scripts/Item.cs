using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public SphereCollider sphereCollider;
    [SerializeField] public CapsuleCollider capusleCollider;
    [SerializeField] public GameObject lightning;
    [SerializeField] public bool hasAbility = false;
    [SerializeField] public bool stackable = false;
    [SerializeField] public bool isReusable = true;
    [SerializeField] public int numberOfUses = -1;

    public virtual void UseAbility()
    {
        //Enter ability code
    }
}
