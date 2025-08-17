using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;

public class Item
{
    public String name;
    public int amount;
    public GameObject? model;
    public GameObject? Icon;
    public Vector2 grid_pos;

    public Item(String name, int amount)
    {
        this.name = name;
        this.amount = amount;
        this.grid_pos = new Vector2(0,0);
    }
}
