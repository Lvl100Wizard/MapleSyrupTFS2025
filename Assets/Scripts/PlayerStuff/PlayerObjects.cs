using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{
    [SerializeField] public Transform holdPoint;
    [SerializeField] public float stackHeight = 0.5f;
    private List<GameObject> heldItems = new List<GameObject>();

    // Collects a new item and stacks it properly
    public void CollectItem(GameObject itemPrefab)
    {
        GameObject newItem = Instantiate(itemPrefab, holdPoint);
        Vector3 stackPosition = heldItems.Count > 0 ?
            heldItems[heldItems.Count - 1].transform.localPosition + new Vector3(0, stackHeight, 0) : Vector3.zero;

        newItem.transform.localPosition = stackPosition;
        heldItems.Add(newItem);
    }

    // ? Drop off a specific number of items of a given type (e.g., "Sap")
    public void DropOffItems(int itemCount, string itemTag)
    {
        int droppedCount = 0;
        heldItems.RemoveAll(item =>
        {
            if (droppedCount < itemCount && item.CompareTag(itemTag))
            {
                Destroy(item);
                droppedCount++;
                return true; // Remove item from list
            }
            return false;
        });

        UpdateStackPositions();


        Debug.Log($"{droppedCount} {itemTag} items dropped off.");
    }

    // ? Drop all items in inventory
    public void DropAllItems()
    {
        foreach (GameObject item in heldItems)
        {
            Destroy(item);
        }
        heldItems.Clear();
    }

    // ? Get count of held items by type (e.g., "Sap")
    public int GetItemCountByTag(string tag)
    {
        int count = 0;
        foreach (GameObject item in heldItems)
        {
            if (item.CompareTag(tag))
            {
                count++;
            }
        }
        return count;
    }

    // ? Check if the player has a specific item
    public bool HasItem(string tag)
    {
        return heldItems.Exists(item => item.CompareTag(tag));
    }

    // ? Get total held item count
    public int GetHeldItemCount()
    {
        return heldItems.Count;
    }

    private void UpdateStackPositions()
    {
        for (int i = 0; i < heldItems.Count; i++)
        {
            heldItems[i].transform.localPosition = new Vector3(0, i * stackHeight, 0);
        }
    }
}
