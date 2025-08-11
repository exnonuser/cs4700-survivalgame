using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public Camera plr_camera;
    public RaycastHit? current_obj_looked;
    private const float MAX_INTERACT_DISTANCE = 7f;

    void interact()
    {
        Debug.Log("Clicked E!");
        if (current_obj_looked?.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            String obj_name = current_obj_looked?.collider.gameObject.name;
            Debug.Log("Picking up " + obj_name);

            InventorySystem.Instance.AddItem(obj_name, 1);

            Destroy(current_obj_looked?.collider.gameObject);
        }

        if (current_obj_looked?.collider.gameObject.CompareTag("Food") == true)
        {
            PlayerState.Instance.currentHunger += 20f; // increase hunger
            PlayerState.Instance.currentHydration += 20f; // increase hunger

            // Make sure values stay within max and min
            PlayerState.Instance.bound_state(ref PlayerState.Instance.currentHunger, ref PlayerState.Instance.maxHunger);
            PlayerState.Instance.bound_state(ref PlayerState.Instance.currentHydration, ref PlayerState.Instance.maxHydration);
        }
        
    }

    RaycastHit fire_ray_from_camera()
    {
        Ray raycast = plr_camera.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        Physics.Raycast(raycast, out hit, MAX_INTERACT_DISTANCE);
        return hit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            interact();
        }
    }

    void FixedUpdate()
    {
        // Fire rays every tick for hovering over interactable items
        RaycastHit? hit_obj = fire_ray_from_camera();
        if (current_obj_looked?.collider == hit_obj?.collider)
        {
            // Checks if we are looking at a previously seen object
            Transform interactionName = hit_obj?.transform?.Find("InteractionName");
            if (interactionName != null && !interactionName.gameObject.activeSelf)
            {
                interactionName.gameObject.SetActive(true); 
            }

        }
        else if (current_obj_looked?.collider != hit_obj?.collider)
        {
            // Looking at a different object than before
            
            Transform interactionName = current_obj_looked?.transform?.Find("InteractionName");
            if (interactionName != null && interactionName.gameObject.activeSelf)
            {
                interactionName.gameObject.SetActive(false); 
            }
        }
        

        current_obj_looked = hit_obj;
        
    }
}
