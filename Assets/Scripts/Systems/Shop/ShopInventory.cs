using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShopInventory", order = 1)]
public class ShopInventory : AbstractSOContainer
{
    public Dictionary<ItemTypes.Types, int> inventoryDict;

    public void Initialize()
    {
        if (inventoryDict == null)
        {
            inventoryDict = new Dictionary<ItemTypes.Types, int>();
        }
    }
    
    public void SetQuantity(ItemTypes.Types itemType, int quantity)
    {        
        if (inventoryDict == null)
        {
            Initialize();
        }

        if (inventoryDict.ContainsKey(itemType))
        {
            inventoryDict[itemType] = quantity;
        }
        else
        {
            inventoryDict.Add(itemType, quantity);
        }
    }

    public void ModifyQuantity(ItemTypes.Types itemType, int quantityChange)
    {
        // Ensure inventoryDict is initialized
        if (inventoryDict == null)
        {
            Initialize();
        }

        UnityEngine.Debug.Log($"modify quantity of {itemType} {quantityChange}");
        foreach (KeyValuePair<ItemTypes.Types, int> item in inventoryDict)
        {
            // item.Key gives you the ItemTypes.Types (the item type)
            // item.Value gives you the quantity (the int value)
            UnityEngine.Debug.Log($"Item: {item.Key}, Quantity: {item.Value}");
        }
        if (inventoryDict.ContainsKey(itemType))
        {
            inventoryDict[itemType] += quantityChange;
        }
    }

    public int GetQuantity(ItemTypes.Types itemType)
    {
        // Ensure inventoryDict is initialized
        if (inventoryDict == null)
        {
            Initialize();
        }

        if (inventoryDict.ContainsKey(itemType))
        {
            return inventoryDict[itemType];
        }
        else
        {
            return 0;  // Return 0 if item doesn't exist in inventory
        }
    }
}