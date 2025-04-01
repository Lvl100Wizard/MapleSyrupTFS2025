using UnityEngine;

public class Boiler : MonoBehaviour, IDropOffHandler, IPickUpHandler
{
    public bool isProducing = false;
    public bool hasSyrup = false;
    public float productionTime = 10.0f;

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
        timerUI.Initialize(this.transform, !isProducing);
    }

    public void HandleDropOff(PlayerObjects player)
    {
        PlayerObjects playerInventory = player.GetComponent<PlayerObjects>();

        // Prevent refueling if syrup is still waiting for pickup
        if (playerInventory != null && playerInventory.GetItemCountByTag("Sap") >= 4 && !isProducing && !hasSyrup)
        {
            Debug.Log("Boiler fueled! Starting production.");
            playerInventory.DropOffItems(4, "Sap"); // Corrected call
            StartProduction();
        }
        else if (hasSyrup)
        {
            Debug.Log("Cannot refuel: Syrup is still waiting for pickup!");
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
                timerUI.SetCheckmarkVisibility(false);
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
