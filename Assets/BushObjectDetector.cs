using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushObjectDetector : MonoBehaviour
{
    [SerializeField] Material invisLeaves;
    [SerializeField] Material regularLeaves;

    Renderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        var type = other.GetType();
        if (type == typeof(CapsuleCollider) && other.gameObject.name.Contains("Farmer"))
        {
            meshRenderer.material = invisLeaves;
        }
        else if (type == typeof(CapsuleCollider) && other.gameObject.name.Contains("Cow"))
        {
            meshRenderer.material = invisLeaves;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            meshRenderer.material = invisLeaves;
        }
        else if (type == typeof(CapsuleCollider) && other.CompareTag("Item"))
        {
            meshRenderer.material = invisLeaves;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var type = other.GetType();
        if (type == typeof(CapsuleCollider) && other.gameObject.name.Contains("Farmer"))
        {
            meshRenderer.material = regularLeaves;
        }
        else if (type == typeof(CapsuleCollider) && other.gameObject.name.Contains("Cow"))
        {
            meshRenderer.material = regularLeaves;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            meshRenderer.material = regularLeaves;
        }
        else if (type == typeof(CapsuleCollider) && other.CompareTag("Item"))
        {
            meshRenderer.material = regularLeaves;
        }
    }
}
