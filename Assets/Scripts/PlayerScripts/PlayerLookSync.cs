using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookSync : MonoBehaviour
{
    public Transform cameraTransform; // assign your camera here in the Inspector
    public Transform ConstructionHolder;

    void Update()
    {
        // Get the camera's Y rotation (horizontal look)
        float cameraY = cameraTransform.eulerAngles.y;

        // Apply it to the player (preserve current X and Z)
        transform.rotation = Quaternion.Euler(0, cameraY, 0);
        
        ConstructionHolder.rotation = cameraTransform.rotation;
    }
}
