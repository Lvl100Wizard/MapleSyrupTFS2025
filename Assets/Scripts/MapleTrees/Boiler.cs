using UnityEngine;

public class Boiler : MonoBehaviour, IDropOffHandler, IPickUpHandler
{
    [Header("Production Variables")]
    public bool isProducing = false;
    public bool hasSyrup = false;
    public float productionTime = 10.0f;
    public int maxSapRequired = 4;
    private int currentSapCount = 0;

    //UI Prefabs
    public GameObject timerUIPrefab;
    public GameObject dropOffRequirementUIPrefab;

    //Syrup Prefab
    public GameObject syrupPrefab;

    private SapTimerUI timerUI;
    private DropOffRequirementUI dropOffUI;
    private Canvas mainCanvas;

    [Header("Item Requirement Icon")]
    public Sprite sapIcon; //Assign in the Inspector

    private void Start()
    {
        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        //Instantiate Timer UI
        GameObject timerUIObject = Instantiate(timerUIPrefab, mainCanvas.transform);
        timerUI = timerUIObject.GetComponent<SapTimerUI>();

        if (timerUI == null)
        {
            Debug.LogError("SapTimerUI component not found on prefab!");
            return;
        }
        timerUI.Initialize(this.transform, isProducing);

        //Instantiate Drop-Off UI
        GameObject dropOffUIObject = Instantiate(dropOffRequirementUIPrefab, mainCanvas.transform);
        dropOffUI = dropOffUIObject.GetComponent<DropOffRequirementUI>();

        if (dropOffUI == null)
        {
            Debug.LogError("DropOffRequirementUI component not found on prefab!");
            return;
        }

        dropOffUI.Initialize(this.transform, sapIcon, maxSapRequired);
    }

    public void HandleDropOff(PlayerObjects playerInventory)
    {
        if (isProducing || hasSyrup)
        {
            Debug.Log("Cannot refuel: Boiler is already producing or syrup is waiting for pickup!");
            return;
        }

        if (playerInventory == null) return;

        int sapHeld = playerInventory.GetItemCountByTag("Sap");

        if (sapHeld > 0)
        {
            int sapNeeded = maxSapRequired - currentSapCount;
            int sapToDeposit = Mathf.Min(sapHeld, sapNeeded);

            playerInventory.DropOffItems(sapToDeposit, "Sap", this.transform);
            currentSapCount += sapToDeposit;

            Debug.Log($"Sap added! Current count: {currentSapCount}/{maxSapRequired}");

            // Update Drop-Off UI
            dropOffUI.UpdateDropOffProgress(currentSapCount, maxSapRequired);

            // Hide Drop-Off UI when requirement is met
            if (currentSapCount >= maxSapRequired)
            {
                Debug.Log("Boiler fueled! Starting production.");
                StartProduction();
                dropOffUI.SetVisible(false); // Hide UI
            }
        }
        else
        {
            Debug.Log("No sap to drop off!");
        }
    }

    public void HandlePickup(PlayerObjects playerInventory)
    {
        if (hasSyrup && playerInventory != null)
        {
            Debug.Log("Player picked up syrup!");
            playerInventory.CollectItem(syrupPrefab);
            hasSyrup = false;
            currentSapCount = 0;

            timerUI.SetCheckmarkVisibility(false);
            timerUI.SetSliderValue(0f);

            // Reset the Drop-Off UI and make it visible again
            dropOffUI.UpdateDropOffProgress(currentSapCount, maxSapRequired);
            dropOffUI.SetVisible(true); // Show UI again
        }
    }

    private void StartProduction()
    {
        isProducing = true;
        timerUI.SetCheckmarkVisibility(false);
        timerUI.StartCooldown(productionTime, FinishProduction);
    }

    private void FinishProduction()
    {
        isProducing = false;
        hasSyrup = true;
        timerUI.SetCheckmarkVisibility(true);
        Debug.Log("Syrup is ready for pickup!");
    }
}
