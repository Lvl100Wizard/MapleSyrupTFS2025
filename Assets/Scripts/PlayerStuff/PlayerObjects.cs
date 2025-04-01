using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{

   [SerializeField] public Transform holdPoint;
    [SerializeField] public float stackHeight = 0.5f;
    private List<GameObject> heldItems = new List<GameObject>();
    // Start is called before the first frame update
  public void CollectItem(GameObject itemPrefab)
    {
        GameObject newItem = Instantiate(itemPrefab, holdPoint);
        Vector3 stackPosition = heldItems.Count > 0 ?
            heldItems[heldItems.Count - 1].transform.localPosition + new Vector3(0, stackHeight, 0) : Vector3.zero;

        newItem.transform.localPosition = stackPosition;
        heldItems.Add(newItem);
    }

   
    public void DropOffItems()
    {

        //this will need more conditions, for now it just clears the whole stack
        foreach (GameObject item in heldItems)
        {
            Destroy(item);
        }
        heldItems.Clear();
    }
}
