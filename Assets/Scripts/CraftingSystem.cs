using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    public GameObject constructionScreenUI;
    [SerializeField] private List<GameObject> craftingUICollection = new List<GameObject>();
    public GameObject currentCraftingUI;

    public List<string> inventoryItemList = new List<string>();

    //category buttons
    public Button toolsBtn; 
    public Button constructionBtn;

    //craft buttons
    public Button craftToolButton;

    //requirement text
    public TextMeshProUGUI GenericReq1, GenericReq2;

    public bool isOpen;
    
    //All blueprint
    public ItemBlueprint AxeBLP = new ItemBlueprint("Axe", 2, "Stone", 3, "Stick", 3);
    public ItemBlueprint PickaxeBLP = new ItemBlueprint("Pickaxe", 1, "Stick", 4);
    public ItemBlueprint FoundationBLP = new ItemBlueprint("Foundation", 1, "Stick", 4);
    public ItemBlueprint WallBLP = new ItemBlueprint("Wall", 1, "Stick", 2);
    public ItemBlueprint BonfireBLP = new ItemBlueprint("Bonfire", 2, "Stick", 2, "Coal", 2);


    public static CraftingSystem Instance {get;set;}

    public Dictionary<string, ItemBlueprint> itemMap;

    private void Awake() {
        craftingUICollection.Add(craftingScreenUI);
        craftingUICollection.Add(toolsScreenUI);
        craftingUICollection.Add(constructionScreenUI);

        itemMap = new Dictionary<string, ItemBlueprint>()
        {
            { "Axe", AxeBLP },
            { "Foundation", FoundationBLP },
            { "Wall", WallBLP },
            { "Pickaxe", PickaxeBLP },
            { "Bonfire", BonfireBLP}
        };

        if (Instance != null && Instance != this) 
        {
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

        // Set up category buttons
        Button toolsBtn = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBtn.onClick.AddListener(OpenToolsCategory);

        Button constructionBtn = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBtn.onClick.AddListener(OpenConstructionCategory);


    }

    void SetupChildrenUI (GameObject currentCraftingUI) {
                // Set up all crafting buttons dynamically
        foreach (Transform child in currentCraftingUI.transform)
        {
            if (child.name == "Title")
                continue; // skip this child
            
            // Find requirement texts and button for this tool
            TextMeshProUGUI req1Text = child.Find("Req1").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI req2Text = child.Find("Req2").GetComponentInChildren<TextMeshProUGUI>();
            Button craftButton = child.Find("Button").GetComponent<Button>();

            // Capture current child for closure to avoid loop capture issue
            Transform currentChild = child;

            craftButton.onClick.AddListener(() =>
            {
                if (itemMap.TryGetValue(currentChild.name, out ItemBlueprint item))
                {
                    CraftAnyItem(item);
                }
            });
        }
    }

    void OpenCategory(GameObject categoryUI)
    {
         foreach (GameObject UI in craftingUICollection)
        {
            UI.SetActive(false);
        }

        categoryUI.SetActive(true);
        currentCraftingUI = categoryUI;

        SetupChildrenUI(currentCraftingUI);
    }

    void OpenToolsCategory ()
    {
        OpenCategory(toolsScreenUI);
    } 

    void OpenConstructionCategory ()
    {
        OpenCategory(constructionScreenUI);
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
        if (isOpen)
            RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            Debug.Log("C pressed");
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            craftingScreenUI.SetActive(true);
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            Debug.Log("C pressed");
            isOpen = false;

            if(!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }

            toolsScreenUI.SetActive(false);
            craftingScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);
        }
    }

    public void RefreshNeededItems()
    {
        var inventoryItemList = InventorySystem.Instance.itemList;
        int haveR1, haveR2 = 0;
        TextMeshProUGUI req1Text, req2Text = null;

        // Count inventory
        Dictionary<string, int> counts = new Dictionary<string, int>();
        foreach (string itemName in inventoryItemList)
        {
            if (!counts.ContainsKey(itemName))
                counts[itemName] = 0;
            counts[itemName]++;
        }

        // Loop through all children
        if (currentCraftingUI != null)
        {
            foreach (Transform child in currentCraftingUI.transform)
            {
                if (child.name == "Title")
                    continue; // skip this child
                if (itemMap.TryGetValue(child.name, out ItemBlueprint currentItem))
                {
                    // Dynamically find each tool's own UI components
                    req1Text = child.Find("Req1").GetComponent<TextMeshProUGUI>();
                    if (child.Find("Req2") != null)
                        req2Text = child.Find("Req2").GetComponent<TextMeshProUGUI>();
                
                    Button craftButton = child.Find("Button").GetComponent<Button>();

                    haveR1 = counts.ContainsKey(currentItem.Req1) ? counts[currentItem.Req1] : 0;
                    req1Text.text = currentItem.Req1Amount + " " + currentItem.Req1 + " [" + haveR1 + "]";

                    if (currentItem.Req2 != null)
                    {
                        haveR2 = counts.ContainsKey(currentItem.Req2) ? counts[currentItem.Req2] : 0;
                        req2Text.text = currentItem.Req2Amount + " " + currentItem.Req2 + " [" + haveR2 + "]";
                    }
                    else 
                        req2Text.text = "";

                    // Show or hide the craft button
                    switch(currentItem.numOfRequirements)
                    {
                        case 1:
                            craftButton.gameObject.SetActive(haveR1 >= currentItem.Req1Amount);
                            break;
                        case 2:
                            craftButton.gameObject.SetActive(haveR1 >= currentItem.Req1Amount && haveR2 >= currentItem.Req2Amount);
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning("No blueprint found for " + child.name);
                }
            }
        }
    }
}
