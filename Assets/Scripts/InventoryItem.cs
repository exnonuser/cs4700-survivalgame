using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
 
public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool isTrashable;
 
    // --- Item Info UI --- //
    private GameObject itemInfoUI;
 
    private TMP_Text itemInfoUI_itemName;
    private TMP_Text itemInfoUI_itemDescription;
    private TMP_Text itemInfoUI_itemFunctionality;
 
    public string thisName, thisDescription, thisFunctionality;
 
    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    // --- Equipable --- //
    public bool isUsable;
    public GameObject itemPendingToBeUsed;

 
    public float healthEffect;
    public float hungerEffect;
    public float hydrationEffect;
 
 
    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<TMP_Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<TMP_Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<TMP_Text>();
    }
 
    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }
 
    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }
 
    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                // Setting this specific gameobject to be the item we want to destroy later
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, hungerEffect, hydrationEffect);
            }
        }

        if (isUsable)
        {
            itemPendingToBeUsed = gameObject;

            UseItem();
        }
    }

    public void UseItem()
    {
        itemInfoUI.SetActive(false);

        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsScreenUI.SetActive(false);
        //CraftingSystem.Instance.survivalScreenUI.SetActive(false);
        //CraftingSystem.Instance.refineScreenUI.SetActive(false);
        CraftingSystem.Instance.constructionScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationalModel");
                break;
            case "Foundation":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationalModel");
                break;
            default:
                break;
        }

    }
 
    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.RecalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }

            if (isUsable && itemPendingToBeUsed == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.RecalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }
 
    private void consumingFunction(float healthEffect, float hungerEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);
 
        healthEffectCalculation(healthEffect);
 
        hungerEffectCalculation(hungerEffect);
 
        hydrationEffectCalculation(hydrationEffect);
 
    }
 
 
    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //
 
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;
 
        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }
 
 
    private static void hungerEffectCalculation(float hungerEffect)
    {
        // --- hunger --- //
 
        float hungerBeforeConsumption = PlayerState.Instance.currentHunger;
        float maxHunger = PlayerState.Instance.maxHunger;
 
        if (hungerEffect != 0)
        {
            if ((hungerBeforeConsumption + hungerEffect) > maxHunger)
            {
                PlayerState.Instance.setHunger(maxHunger);
            }
            else
            {
                PlayerState.Instance.setHunger(hungerBeforeConsumption + hungerEffect);
            }
        }
    }
 
 
    private static void hydrationEffectCalculation(float hydrationEffect)
    {
        // --- hydration --- //
 
        float hydrationBeforeConsumption = PlayerState.Instance.currentHydration;
        float maxHydration = PlayerState.Instance.maxHydration;
 
        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.setHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }
 
 
}