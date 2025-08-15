using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance {get;set;}

    public InteractableObject currentTarget {get; set;}
    public bool onTarget => currentTarget != null;

    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;
    public GameObject CrosshairUI;
 
    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponentInChildren<TextMeshProUGUI>();
    }
 
    private void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    void Update()
    {
        UpdateSelection();
    }

    void UpdateSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var interactable = hit.transform.GetComponent<InteractableObject>();
            if (interactable != null && interactable.playerInRange)
            {
                SetTarget(interactable);
                return;
            }
        }
        ClearTarget();
    }

    public void SetTarget(InteractableObject obj)
    {
        currentTarget = obj;
        interaction_text.text = obj.GetItemName();
        interaction_Info_UI.SetActive(true);
    }

    public void ClearTarget()
    {
        currentTarget = null;
        interaction_Info_UI.SetActive(false);
    }

    public void DisableSelection()
    {
        CrosshairUI.SetActive(false);
        interaction_Info_UI.SetActive(false);

        currentTarget = null;
    }

    public void EnableSelection()
    {
        CrosshairUI.SetActive(true);
        interaction_Info_UI.SetActive(true);

    }
}