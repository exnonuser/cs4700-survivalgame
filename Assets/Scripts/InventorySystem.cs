using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;

public class InventorySystem : MonoBehaviour
{
 
   public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;
    public bool isOpen;

    public List<Item> inventory;
    private const int MAX_ROWS = 3;
    private const int MAX_COLS = 8;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        inventory = new List<Item>();
    }

    public void AddItem(String name, int amount)
    {
        // Method for adding items to an inventory
        Item item_obj = new Item(name, amount);
        item_obj.Icon = Resources.Load<GameObject>("Inventory Items/" + name);

        // First check if item already exists in inventory
        if (inventory.Contains(item_obj))
        {
            Item item = inventory.Find(item => item.name == name);
            item.amount += amount;
        }
        else
        {
            inventory.Add(item_obj);
        }
        
    }

    public Item RemoveItem(String name)
    {
        Item item_to_remove = inventory.Find(item => item.name == name);
        inventory.Remove(item_to_remove);
        return item_to_remove;
    }
 
    void Start()
    {
        isOpen = false;
    }

    private void update_inventory()
    {
        int MAX_SLOTS = inventoryScreenUI.transform.childCount;
        // Clear inventory
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            
            Transform slot = inventoryScreenUI.transform.GetChild(i);
            if (slot.childCount > 0) {
                // Store positions into inventory first
                GameObject image = slot.GetChild(0).gameObject;
                int row = (int) Mathf.Floor(i / MAX_COLS);
                int col = i % MAX_COLS;
                Debug.Log("Found" + image.name);

                inventory.Find(item => item.name == image.name).grid_pos = new Vector2(row, col);
                // Remove image from ui
                Destroy(image);
            }
        }

        // Add items from inventory into slots
        int j = 0;
        foreach (Item item in inventory)
        {
            if (j > MAX_SLOTS) break; // Case where inventory is bigger than UI inventory
            int row = (int)item.grid_pos.x;
            int col = (int)item.grid_pos.y;
            Transform slot = inventoryScreenUI.transform.GetChild(row * MAX_COLS + col);
            GameObject icon_copy = Instantiate(item.Icon, slot);
            icon_copy.name = item.name;
            j++;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            Cursor.lockState = CursorLockMode.None;
            inventoryScreenUI.SetActive(true);
            isOpen = true;
            // Update inventory
            update_inventory();
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            inventoryScreenUI.SetActive(false);
            isOpen = false;
        }
    }
 
}