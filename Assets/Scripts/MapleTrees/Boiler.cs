using UnityEngine;

public class Boiler : MonoBehaviour, IDropOffHandler, IPickUpHandler
{
    [Header("Production Variables")]
    public bool isProducing = false;
    public bool hasSyrup = false;
    public float productionTime = 10.0f;
    public int maxSapRequired = 4;
    private int currentSapCount = 0;

    // BoilerTimerUI Prefab
    public GameObject timerUIPrefab;

    // SyrupPail Prefab
    public GameObject syrupPrefab;

    private SapTimerUI timerUI;
    private Canvas mainCanvas;

    private void Start()
    {
        // Find the main canvas
        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        // Instantiate UI for this boiler
        GameObject timerUIObject = Instantiate(timerUIPrefab, mainCanvas.transform);
        timerUI = timerUIObject.GetComponent<SapTimerUI>();

        if (timerUI == null)
        {
            Debug.LogError("SapTimerUI component not found on prefab!");
            return;
        }

        // Initialize UI to follow the boiler
        timerUI.Initialize(this.transform, isProducing);
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
            int sapToDeposit = Mathf.Min(sapHeld, sapNeeded); // Take only what's needed

            playerInventory.DropOffItems(sapToDeposit, "Sap");
            currentSapCount += sapToDeposit;

            Debug.Log($"Sap added! Current count: {currentSapCount}/{maxSapRequired}");

            if (currentSapCount >= maxSapRequired)
            {
                Debug.Log("Boiler fueled! Starting production.");
                StartProduction();
            }
        }
        else
        {
            Debug.Log("No sap to drop off!");
        }
    }


    public void HandlePickup(PlayerObjects playerInventory)
    {
        if (hasSyrup)
        {
            if (playerInventory != null)
            {
                Debug.Log("Player picked up syrup!");
                playerInventory.CollectItem(syrupPrefab);
                hasSyrup = false;
                currentSapCount = 0;
                timerUI.SetCheckmarkVisibility(false);
                timerUI.SetSliderValue(0f);
            }
        }
    }

    private void StartProduction()
    {
        isProducing = true;
        timerUI.SetCheckmarkVisibility(false);
        timerUI.StartCooldown(productionTime, () => FinishProduction());
    }

    private void FinishProduction()
    {
        isProducing = false;
        hasSyrup = true;
        timerUI.SetCheckmarkVisibility(true);
        Debug.Log("Syrup is ready for pickup!");
    }
}
