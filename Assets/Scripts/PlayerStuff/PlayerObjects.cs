using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{
    [SerializeField] public Transform holdPoint;
    [SerializeField] public float stackHeight = 0.5f; //spacing between items on stack
    public float pickupSpeed = 0.5f; // How fast items move to the stack

    private List<GameObject> heldItems = new List<GameObject>();

    // Collects a new item and stacks it properly
    public void CollectItem(GameObject itemPrefab, Transform pickUpPoint)
    {
        GameObject newItem = Instantiate(itemPrefab, pickUpPoint.position, Quaternion.identity);
       
        
        newItem.transform.SetParent(holdPoint, true); // Attach it to player without altering world position

        Vector3 targetLocalPosition = heldItems.Count > 0 
            ? heldItems[heldItems.Count - 1].transform.localPosition + new Vector3(0, stackHeight, 0) 
            : Vector3.zero;

        //newItem.transform.localPosition = targetPosition;
        heldItems.Add(newItem);


        //newItem.transform needs change to item spawners location
        StartCoroutine(MoveToPosition(newItem.transform, targetLocalPosition));
    }

    // ? Drop off a specific number of items of a given type (e.g., "Sap")
    public void DropOffItems(int itemCount, string itemTag, Transform dropOffPoint)
    {
        int droppedCount = 0;
        //new lerp stuff here
        List<GameObject> itemsToRemove = new List<GameObject>();

        foreach (GameObject item in heldItems)
        {
            if (droppedCount < itemCount && item.CompareTag(itemTag))
            {
                itemsToRemove.Add(item);
                droppedCount++;
            }
        }

        foreach (GameObject item in itemsToRemove)
        {
            heldItems.Remove(item);
            StartCoroutine(MoveToDropOff(item.transform, dropOffPoint));
        }
        //end of lerp stuf

        /*  heldItems.RemoveAll(item =>
        {
            if (droppedCount < itemCount && item.CompareTag(itemTag))
            {
                Destroy(item);
                droppedCount++;
                return true; // Remove item from list
            }
            return false;
        }); */

        UpdateStackPositions();
        Debug.Log($"{droppedCount} {itemTag} items dropped off.");
    }


    private IEnumerator MoveToPosition(Transform item, Vector3 targetLocalPosition)
    {
        Vector3 startLocalPosition = item.localPosition;
        //Vector3 worldTargetPosition = holdPoint.TransformPoint(targetPosition); // Convert local to world position

        float elapsedTime = 0;

        while (elapsedTime < pickupSpeed)
        {
            float t = elapsedTime / pickupSpeed;
            t = t * t * (3f - 2f * t); // Smoothstep easing
            item.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.localPosition = targetLocalPosition;
    }

    private IEnumerator MoveToDropOff(Transform item, Transform dropOffPoint)
    {
        Vector3 startPosition = item.position;
        Vector3 targetPosition = dropOffPoint.position;
        float elapsedTime = 0;

        while (elapsedTime < pickupSpeed)
        {
            float t = elapsedTime / pickupSpeed;
            t = t * t * (3f - 2f * t); // Smoothstep easing
            item.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.position = targetPosition;
        Destroy(item.gameObject);
    }


    // ? Drop all items in inventory
    //not used at the moment
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
