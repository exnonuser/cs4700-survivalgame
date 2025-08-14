using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    public GameObject CrosshairUI;
    public List<string> inventoryItemList = new List<string>();

    //category buttons
    public Button toolsBtn; 

    //craft buttons
    public Button craftAxeButton;

    //requirement text
    public TextMeshProUGUI AxeReq1, AxeReq2;

    public bool isOpen;
    
    //All blueprint
    public ItemBlueprint AxeBLP;

    public static CraftingSystem Instance {get;set;}

    private void Awake() {
        AxeBLP = new ItemBlueprint("Axe", 2, "Stone", 3, "Stick", 3);

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        toolsBtn = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBtn.onClick.AddListener(delegate{OpenToolsCategory();});

        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponentInChildren<TextMeshProUGUI>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponentInChildren<TextMeshProUGUI>();

        craftAxeButton = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeButton.onClick.AddListener(delegate{CraftAnyItem(AxeBLP);});
    }

    void OpenToolsCategory ()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    } 

    void CraftAnyItem (ItemBlueprint blueprintToCraft) 
    {
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        if (blueprintToCraft.numOfRequirements == 1) 
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        }

        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }

        StartCoroutine(calculate());
        RefreshNeededItems();
    }

    public IEnumerator calculate()
    {
        //yield return new WaitForSeconds(1f);
        yield return null;
        InventorySystem.Instance.RecalculateList();
    }


    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            Debug.Log("C pressed");
            isOpen = true;
            CrosshairUI.SetActive(!isOpen); //Crosshair is off when crafting is on
            Cursor.lockState = CursorLockMode.None;
            craftingScreenUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            Debug.Log("C pressed");
            isOpen = false;

            if(!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                CrosshairUI.SetActive(!isOpen);
            }

            toolsScreenUI.SetActive(false);
            craftingScreenUI.SetActive(false);
        }
    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string itemName in inventoryItemList)
        {
            switch(itemName)
            {
                case ("Stone"):
                {
                    stone_count += 1;
                    break;
                }
                case ("Stick"):
                {
                    stick_count += 1;
                    break;
                }
            }
        }

        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeButton.gameObject.SetActive(true);
        }

        else
        {
            craftAxeButton.gameObject.SetActive(false);
        }

    }
}
