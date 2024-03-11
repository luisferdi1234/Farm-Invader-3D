using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityCloakManager : MonoBehaviour
{
    //Alien variables
    GameObject alienMesh;
    [SerializeField] Material alienSkin;
    [SerializeField] Material invisibilityMaterial;
    SkinnedMeshRenderer skinnedMeshRenderer;

    //Timer variables
    float invisTime = 0;
    [HideInInspector] public float maxInvisTime;
    bool InvisInUse;

    private void Awake()
    {
        alienMesh = GameObject.Find("AlienMesh");
        skinnedMeshRenderer = alienMesh.GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        UpdateInvisibility();
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

    public void TurnOnInvisibility()
    {
        InvisInUse = true;
        alienMesh.transform.parent.tag = "Invisible";
        invisTime = 0;
        AudioManager.instance.PlaySpecificSound("AlienCloak", 1f);
        //Changes Material
        ChangePlayerMaterial(invisibilityMaterial);
    }

    /// <summary>
    /// Handles all the variable changes associated with invisibility running out
    /// </summary>
    public void TurnOffInvisibility()
    {
        alienMesh.transform.parent.tag = "Player";
        alienMesh.transform.parent.GetComponent<Inventory>().invisibilityCloakManager = null;
        InvisInUse = false;
        ChangePlayerMaterial(alienSkin);
        invisTime = 0;
        Destroy(gameObject);
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
