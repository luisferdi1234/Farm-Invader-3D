using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CloakItem : Item
{
    //Cloak variables
    [SerializeField] GameObject cloakManager;
    InvisibilityCloakManager invisibilityCloakManager;
    [SerializeField] float maxInvisTime = 3f;

    private GameObject Alien;
    public override void Start()
    {
    }

    public override void Update()
    {
        
    }

    public override void UseAbility()
    {
        if (energy >= maxEnergy)
        {
            GameObject curr = Instantiate(cloakManager);
            curr.transform.parent = transform;
            invisibilityCloakManager = curr.GetComponent<InvisibilityCloakManager>();
            transform.parent.GetComponent<Inventory>().invisibilityCloakManager = invisibilityCloakManager;
            invisibilityCloakManager.maxInvisTime = maxInvisTime;
            invisibilityCloakManager.TurnOnInvisibility();
            energy = 0;
        }
    }

    public override void TurnOffAbility()
    {
        invisibilityCloakManager.TurnOffInvisibility();
        invisibilityCloakManager = null;
    }
}
