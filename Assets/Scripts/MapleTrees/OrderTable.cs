using UnityEngine;
using System.Collections.Generic;

public class OrderTable : MonoBehaviour, IDropOffHandler
{
    private Dictionary<string, int> currentItemCounts = new Dictionary<string, int>();
    private OrderData currentOrder;

    [SerializeField] private AudioClip dropOffClip;
    public GameObject dropOffRequirementUIPrefab;

    private DropOffRequirementUI dropOffUI;
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

        GameObject dropOffUIObject = Instantiate(dropOffRequirementUIPrefab, gameUIPanelTransform);
        dropOffUI = dropOffUIObject.GetComponent<DropOffRequirementUI>();

        if (dropOffUI == null)
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
            currentItemCounts.Clear();
            foreach (var requirement in currentOrder.requiredItems)
            {
                currentItemCounts[requirement.itemTag] = 0;
            }

            dropOffUI.Initialize(this.transform, currentOrder.requiredItems[0].icon, currentOrder.requiredItems[0].requiredAmount);
            dropOffUI.SetVisible(true);
        }
        else
        {
            Debug.Log("No more orders.");
            dropOffUI.SetVisible(false);
        }
    }

    public void HandleDropOff(PlayerObjects playerInventory)
    {
        if (currentOrder == null || playerInventory == null) return;

        bool anyItemDeposited = false;

        foreach (var requirement in currentOrder.requiredItems)
        {
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
                    dropOffUI.UpdateDropOffProgress(currentItemCounts[requirement.itemTag], requirement.requiredAmount);
                }
            }
        }

        if (IsOrderComplete())
        {
            Debug.Log("Order complete!");
            dropOffUI.SetVisible(false);
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
