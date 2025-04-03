using UnityEngine;

public class FarmShop : MonoBehaviour, IDropOffHandler, INPCPickUpHandler
{
    [Header("Production Variables")]    
    //UI Prefabs
    public GameObject dropOffRequirementUIPrefab;
    [SerializeField] private ShopInventory inventory;
    private DropOffRequirementUI dropOffUI;
    private int maxSyrupRequired = 100;
    private int currentSyrupCount = 0; 
    public string displaySyrupCount = "0";

    [SerializeField] private Wallet playerWallet;

    [Header("Item Requirement Icon")]
    public Sprite sapIcon; //Assign in the Inspector

    private Canvas mainCanvas;

    private void Start()
    {
        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        GameObject dropOffUIObject = Instantiate(dropOffRequirementUIPrefab, mainCanvas.transform);
        dropOffUI = dropOffUIObject.GetComponent<DropOffRequirementUI>();
        
        if (dropOffUI == null)
        {
            UnityEngine.Debug.LogError("DropOffRequirementUI component not found on prefab!");
            return;
        }

        dropOffUI.Initialize(this.transform, sapIcon, maxSyrupRequired); //TODO: update to syrup icon if one is available
    }

    public void HandleDropOff(PlayerObjects playerInventory)
    {
        UnityEngine.Debug.Log($"handle dropoff at farm stand");
        if (playerInventory == null)
        {
            UnityEngine.Debug.Log("Player inventory is null");
            return;
        }

        int syrupHeld = playerInventory.GetItemCountByTag("Syrup");
        UnityEngine.Debug.Log($"Player has {syrupHeld} syrup");
        if (syrupHeld > 0)
        {
            playerInventory.DropOffItems(syrupHeld, "Syrup", this.transform);
            currentSyrupCount += syrupHeld;
            inventory.currentStockDict[ItemTypes.Types.SyrupUnfiltered] += syrupHeld;
            

            UnityEngine.Debug.Log($"Syrup dropped off at farm stand! Current count: {currentSyrupCount}");
            UnityEngine.Debug.Log($"Syrup dropped off at farm stand! Current inventory: {inventory.currentStockDict[ItemTypes.Types.SyrupUnfiltered]}");

            // Update Drop-Off UI
            //dropOffUI.UpdateDropOffProgress(currentSapCount, maxSapRequired);
            displaySyrupCount = currentSyrupCount.ToString();

        }
        else
        {
            UnityEngine.Debug.Log("No syrup to drop off!");
        }
    }
    /* TODO: update to handle NPC pickups */
    public void HandlePickup()
    {
        UnityEngine.Debug.Log("NPC came to pick up syrup!");
        if (currentSyrupCount > 0)
        {
            UnityEngine.Debug.Log("NPC picked up syrup!");
            currentSyrupCount--;
            displaySyrupCount = currentSyrupCount.ToString();
            playerWallet.GetMoney(20.0f);
            UnityEngine.Debug.Log($"player wallet contains {playerWallet.Money}");
        }
    }

}
