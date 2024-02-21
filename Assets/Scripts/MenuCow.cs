using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCow : MonoBehaviour
{
    public float warpDistance = 1f; // Distance to warp the object
    public float initialSpeed = 5f;
    private Vector3 warpDirection; // Randomized warp direction on startup
    private Rigidbody rb;

    void Start()
    {
        // Randomize the warp direction on startup
        warpDirection = Random.insideUnitCircle.normalized;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Give the object an initial burst of movement in x and y directions
        rb.velocity = new Vector3(warpDirection.x, warpDirection.y, 0) * initialSpeed;
    }

    void Update()
    {
        if (rb.velocity.magnitude == 0)
        {
            // Randomize the warp direction on startup
            warpDirection = Random.insideUnitCircle.normalized;
            // Give the object an initial burst of movement in x and y directions
            rb.velocity = new Vector3(warpDirection.x, warpDirection.y, 0) * initialSpeed;
        }
        // Check if the object is outside of the camera space
        if (!IsInCameraView())
        {
            // Warp the object in the randomized direction
            WarpObject();
        }
    }

    bool IsInCameraView()
    {
        // Check if the object is within the camera's view frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }

    void WarpObject()
    {
        // Get the screen position of the object
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // Wrap the object to the opposite side based on which side it exited
        if (screenPos.x < 0)
        {
            screenPos.x = Screen.width;
        }
        else if (screenPos.x > Screen.width)
        {
            screenPos.x = 0;
        }

        if (screenPos.y < 0)
        {
            screenPos.y = Screen.height;
        }
        else if (screenPos.y > Screen.height)
        {
            screenPos.y = 0;
        }

        // Set the new position in world space
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);
    }
}
