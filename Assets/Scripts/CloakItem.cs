using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CloakItem : Item
{
    //Cloak variables
    [HideInInspector] public float invisTime = 0;
    [HideInInspector] public float invisibilityCooldown = 0f;
    [SerializeField] Material alienSkin;
    [SerializeField] Material invisibilityMaterial;
    [SerializeField] float maxInvisTime = 3f;
    private bool InvisInUse = false;
    private bool startInvisCooldown = false;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private GameObject Alien;
    public override void Start()
    {
        Alien = gameObject.transform.parent.gameObject;
        //Renderer for Invisibility
        skinnedMeshRenderer = Alien.GetComponent<SkinnedMeshRenderer>();
    }

    public override void Update()
    {
        UpdateInvisibility();
    }

    public override void UseAbility()
    {
        InvisInUse = true;
        gameObject.transform.parent.transform.parent.tag = "Invisible";
        energy = 0;
        AudioManager.instance.PlaySpecificSound("AlienCloak", 1f);
        //Changes Material
        ChangePlayerMaterial(invisibilityMaterial);
    }

    public override void TurnOffAbility()
    {
        TurnOffInvisibility();
    }

    /// <summary>
    /// Updates invisibility variables and cooldowns after activation
    /// </summary>
    private void UpdateInvisibility()
    {
        if (InvisInUse)
        {
            invisTime += Time.deltaTime;
        }
        if (invisTime >= maxInvisTime)
        {
            TurnOffInvisibility();
        }
    }

    /// <summary>
    /// Handles all the variable changes associated with invisibility running out
    /// </summary>
    private void TurnOffInvisibility()
    {
        gameObject.transform.parent.transform.parent.tag = "Player";
        InvisInUse = false;
        ChangePlayerMaterial(alienSkin);
        startInvisCooldown = true;
        invisTime = 0;
    }

    /// <summary>
    /// Changes player material to new material
    /// </summary>
    /// <param name="newMaterial"></param>
    private void ChangePlayerMaterial(Material newMaterial)
    {
        // Change the player's material
        skinnedMeshRenderer.material = newMaterial;
    }
}
