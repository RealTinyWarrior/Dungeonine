using System;
using UnityEngine;

[Serializable]
public class Item
{
    [HideInInspector]
    public enum ItemTypes
    {
        Utility,
        Classic
    }
    public string name;
    public int id = 0;
    public Sprite icon;
    public GameObject itemObject;
    public ItemTypes itemType = ItemTypes.Classic;
    public bool stackable;
    [HideInInspector] public int amount = 1;
    [HideInInspector] public int mouseKey = 0;
    [HideInInspector] public GameObject itemReference;

    public Item Clone() => new() { name = name, id = id, icon = icon, itemObject = itemObject, itemType = itemType, stackable = stackable, amount = amount, mouseKey = mouseKey, itemReference = itemReference };
};
