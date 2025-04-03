using System;
using UnityEngine;

public class FarmShop : MonoBehaviour, IDropOffHandler, INPCPickUpHandler
{
    [Header("Production Variables")]    
    //UI Prefabs
    public GameObject dropOffRequirementUIPrefab;
    [SerializeField] private ShopInventory inventory;
    private ItemTypes shopItemTypes = new ItemTypes();
    private ItemCosts priceList;
    [SerializeField] float priceMultiplier = 1.0f;
    private DropOffRequirementUI dropOffUI;
    private int maxSyrupRequired = 100;
    private int currentSyrupCount = 0; 
    public string displaySyrupCount = "0";
    [SerializeField] private int minCustomerItems = 0;
    [SerializeField] private int maxCustomerItems = 10;
    private int totalItemsInShop = 0; //tally of number of items in shop - required for npc pickup to work without infinite loops
    private ItemTypes.Types itemToBuy;
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

        //items to collect at this farm stand
        /*
            Sap,
            SyrupUnfiltered, 
            SyrupFiltered, 
            SyrupBottleUnfiltered,
            SyrupBottleFiltered,
            TaffyTray,
            TaffyBox,
            IceCreamBucket,
        */
        foreach (ItemTypes.Types itemType in System.Enum.GetValues(typeof(ItemTypes.Types)))
        {
            var numHeld = playerInventory.GetItemCountByTag(itemType.ToString());
            if (numHeld > 0)
            {
                playerInventory.DropOffItems(numHeld, itemType.ToString(), this.transform);
                inventory.currentStockDict[itemType] += numHeld;
                UnityEngine.Debug.Log($"{itemType} dropped off at farm stand! Current inventory: {inventory.currentStockDict[itemType]}");
            }
            else
            {
                UnityEngine.Debug.Log($"No {itemType} to drop off!");
            }
        }

    }

    /* TODO: update to handle NPC pickups */
    public void HandlePickup()
    {
        //NPC will roll the number of items they want between min/max       
        int itemsWanted = UnityEngine.Random.Range(minCustomerItems, maxCustomerItems); // Generates a number between 1 and 9
        UnityEngine.Debug.Log($"NPC came to pick up {minCustomerItems}!");

        //iterate over the number of items they want
        for(int i=0; i < itemsWanted; i++)
        {
            //pick a random item
            itemToBuy = shopItemTypes.GetRandomEnumValue();
            UnityEngine.Debug.Log($"NPC wants a {itemToBuy}!");

            //if it's not in stock check for a different item
            if (inventory.currentStockDict[itemToBuy] == 0)
            {
                UnityEngine.Debug.Log($"NPC wants a {itemToBuy}: none in stock!");
                itemToBuy = shopItemTypes.GetRandomEnumValue();
                UnityEngine.Debug.Log($"NPC will try a {itemToBuy} instead if it's in stock but will give up if it's not and move on to their next wanted item!");
            }

            
            //if there is stock buy one, but if not we only check once for an alternate and then keep moving, odd but that's the logic for now
            if (inventory.currentStockDict[itemToBuy] > 0)
            {                
                inventory.currentStockDict[itemToBuy] -= 1;
                float payment = priceList.costDict[itemToBuy] * priceMultiplier;
                UnityEngine.Debug.Log($"NPC buys a {itemToBuy} for {payment} beaver bucks!");
                playerWallet.GetMoney(payment);
            }            
            //iterate items wanted whether the npc got what they wanted or not

            //TODO: may wish to have items wanted progress over the course of the game in the future
        }
    }

    private void UpdateTotalItems()
    {
        var totalItemsCounted = 0;
        foreach (ItemTypes.Types itemType in System.Enum.GetValues(typeof(ItemTypes.Types)))
        {
            totalItemsCounted += inventory.currentStockDict[itemType];
        }
        totalItemsInShop = totalItemsCounted;
    }
}
