using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OrderTable : MonoBehaviour, IDropOffHandler
{
    [SerializeField] public Transform UILeft;
    [SerializeField] public Transform UICenter;
    [SerializeField] public Transform UIRight;


    private Dictionary<string, int> currentItemCounts = new Dictionary<string, int>();
    private OrderData currentOrder;

    [SerializeField] private AudioClip dropOffClip;
    public GameObject dropOffRequirementUIPrefab;

    private DropOffRequirementUI dropOffUI_1;
    private DropOffRequirementUI dropOffUI_2;
    private DropOffRequirementUI dropOffUI_3;

    private Canvas mainCanvas;

    private void Start()
    {
        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        Transform gameUIPanelTransform = mainCanvas.transform.Find("GameUIPanel");
        if (gameUIPanelTransform == null)
        {
            Debug.LogError("No GameUIPanel found inside the Canvas!");
            return;
        }

        GameObject dropOffUIObject_1 = Instantiate(dropOffRequirementUIPrefab, gameUIPanelTransform);
        dropOffUI_1 = dropOffUIObject_1.GetComponent<DropOffRequirementUI>();
        GameObject dropOffUIObject_2 = Instantiate(dropOffRequirementUIPrefab, gameUIPanelTransform);
        dropOffUI_2 = dropOffUIObject_2.GetComponent<DropOffRequirementUI>();
        GameObject dropOffUIObject_3 = Instantiate(dropOffRequirementUIPrefab, gameUIPanelTransform);
        dropOffUI_3 = dropOffUIObject_3.GetComponent<DropOffRequirementUI>();

        if (dropOffUI_1 == null)
        {
            Debug.LogError("DropOffRequirementUI component not found on prefab!");
            return;
        }
        if (dropOffUI_2 == null)
        {
            Debug.LogError("DropOffRequirementUI component not found on prefab!");
            return;
        }
        if (dropOffUI_3 == null)
        {
            Debug.LogError("DropOffRequirementUI component not found on prefab!");
            return;
        }

        UpdateOrder();
    }

    private void UpdateOrder()
    {
        currentOrder = OrderManager.instance.GetCurrentOrder();
        if (currentOrder != null)
        {
            int count = currentOrder.requiredItems.Count();
            currentItemCounts.Clear();

            foreach (var requirement in currentOrder.requiredItems)
            {
                currentItemCounts[requirement.itemTag] = 0;
            }

            // Hide all UIs first
            dropOffUI_1.SetVisible(false);
            dropOffUI_2.SetVisible(false);
            dropOffUI_3.SetVisible(false);

            // Show only the required UI elements
            if (count >= 1)
            {
                dropOffUI_1.Initialize(UILeft, currentOrder.requiredItems[0].icon, currentOrder.requiredItems[0].requiredAmount);
                dropOffUI_1.SetVisible(true);
            }
            if (count >= 2)
            {
                dropOffUI_2.Initialize(UICenter, currentOrder.requiredItems[1].icon, currentOrder.requiredItems[1].requiredAmount);
                dropOffUI_2.SetVisible(true);
            }
            if (count == 3)
            {
                dropOffUI_3.Initialize(UIRight, currentOrder.requiredItems[2].icon, currentOrder.requiredItems[2].requiredAmount);
                dropOffUI_3.SetVisible(true);
            }
        }
        else
        {
            Debug.Log("No more orders.");
            dropOffUI_1.SetVisible(false);
            dropOffUI_2.SetVisible(false);
            dropOffUI_3.SetVisible(false);
        }
    }


    public void HandleDropOff(PlayerObjects playerInventory)
    {
        if (currentOrder == null || playerInventory == null) return;

        bool anyItemDeposited = false;

        for (int i = 0; i < currentOrder.requiredItems.Count(); i++)
        {
            var requirement = currentOrder.requiredItems[i];
            int heldItems = playerInventory.GetItemCountByTag(requirement.itemTag);
            if (heldItems > 0)
            {
                int needed = requirement.requiredAmount - currentItemCounts[requirement.itemTag];
                int toDeposit = Mathf.Min(heldItems, needed);

                if (toDeposit > 0)
                {
                    playerInventory.DropOffItems(toDeposit, requirement.itemTag, this.transform);
                    currentItemCounts[requirement.itemTag] += toDeposit;
                    anyItemDeposited = true;

                    SoundFXManager.instance.PlaySoundFXClip(dropOffClip, transform, 1f);

                    // Update the correct UI element
                    if (i == 0) dropOffUI_1.UpdateDropOffProgress(currentItemCounts[requirement.itemTag], requirement.requiredAmount);
                    if (i == 1) dropOffUI_2.UpdateDropOffProgress(currentItemCounts[requirement.itemTag], requirement.requiredAmount);
                    if (i == 2) dropOffUI_3.UpdateDropOffProgress(currentItemCounts[requirement.itemTag], requirement.requiredAmount);
                }
            }
        }

        if (IsOrderComplete())
        {
            Debug.Log("Order complete!");
            dropOffUI_1.SetVisible(false);
            dropOffUI_2.SetVisible(false);
            dropOffUI_3.SetVisible(false);
            OrderManager.instance.CompleteOrder();
            UpdateOrder();
        }
    }


    private bool IsOrderComplete()
    {
        foreach (var requirement in currentOrder.requiredItems)
        {
            if (currentItemCounts[requirement.itemTag] < requirement.requiredAmount)
                return false;
        }
        return true;
    }
}