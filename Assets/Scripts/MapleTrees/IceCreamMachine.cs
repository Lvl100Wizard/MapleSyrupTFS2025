
using UnityEngine;

public class IceCreamMachine : BaseProductionMachine
{
    [SerializeField] private AudioClip dropOffClip;
    [SerializeField] private AudioClip pickupClip;
    public MeshRenderer IceCreamMesh; //child component
    public GameObject IceCreamPrefab;
    public Sprite sapIcon;
    public Sprite taffyIcon;
    public Transform Uipos1;
    public Transform Uipos2;
    [SerializeField] protected override string RequiredItemTag() => "";
    protected override GameObject OutputPrefab() => IceCreamPrefab;

    protected override MeshRenderer childMesh() => IceCreamMesh;


    protected override AudioClip DropOffClip => dropOffClip;
    protected override AudioClip PickupClip => pickupClip;

    protected DropOffRequirementUI taffyDropOffUI;
    protected DropOffRequirementUI sapDropOffUI;



    private int sapCount = 0;
    private int taffyCount = 0;

    protected override int maxInputRequired { get { return 6; } } // Total required (4 Sap + 2 Taffy)

    public override void InitializeUI()
    {

        if (sapDropOffUI != null && taffyDropOffUI != null)
            return;

        mainCanvas = FindObjectOfType<Canvas>();
        if (!mainCanvas) { Debug.LogError("No Canvas found!"); return; }

        Transform gameUIPanel = mainCanvas.transform.Find("GameUIPanel");
        if (!gameUIPanel) { Debug.LogError("No GameUIPanel found in Canvas!"); return; }

        // Initialize Sap UI
        GameObject sapUIObject = Instantiate(dropOffRequirementUIPrefab, gameUIPanel);
        sapDropOffUI = sapUIObject.GetComponent<DropOffRequirementUI>();
        sapDropOffUI.Initialize(Uipos2, sapIcon, 4);  // Use correct sap icon

        // Initialize Toffee UI
        GameObject toffeeUIObject = Instantiate(dropOffRequirementUIPrefab, gameUIPanel);
        taffyDropOffUI = toffeeUIObject.GetComponent<DropOffRequirementUI>();
        taffyDropOffUI.Initialize(Uipos1, taffyIcon, 2); // Use correct toffee icon

        // Initialize Timer UI
        GameObject timerUIObject = Instantiate(timerUIPrefab, gameUIPanel);
        timerUI = timerUIObject.GetComponent<SapTimerUI>();
        timerUI.Initialize(transform, isProducing);
    }


    public override void HandleDropOff(PlayerObjects playerInventory)
    {
        if (isProducing || hasOutput) return;

        int sapHeld = playerInventory.GetItemCountByTag("Sap");
        int taffyHeld = playerInventory.GetItemCountByTag("TaffyTray");

        // Deposit Sap first
        if (sapHeld > 0 && sapCount < 4)
        {
            int sapToDeposit = Mathf.Min(sapHeld, 4 - sapCount);
            playerInventory.DropOffItems(sapToDeposit, "Sap", transform);
            sapCount += sapToDeposit;
        }

        // Deposit Taffy Trays next
        if (taffyHeld > 0 && taffyCount < 2)
        {
            int taffyToDeposit = Mathf.Min(taffyHeld, 2 - taffyCount);
            playerInventory.DropOffItems(taffyToDeposit, "TaffyTray", transform);
            taffyCount += taffyToDeposit;
        }

        // Update UI to show separate progress
        sapDropOffUI.UpdateDropOffProgress(sapCount, 4);  // Update Sap progress
        taffyDropOffUI.UpdateDropOffProgress(taffyCount, 2); // Update Taffy progress

        // If both inputs are complete, start production
        if (sapCount >= 4 && taffyCount >= 2)
        {
            StartProduction();
            dropOffUI.SetVisible(false);
        }
    }

    protected override void StartProduction()
    {
        base.StartProduction();

        // Clear the stored ingredient counts once production starts
        sapCount = 0;
        taffyCount = 0;
        sapDropOffUI.SetVisible(false);
        taffyDropOffUI.SetVisible(false);
    }

    

  

}
