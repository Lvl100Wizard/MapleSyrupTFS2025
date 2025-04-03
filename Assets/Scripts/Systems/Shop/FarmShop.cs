using System;
using UnityEngine;
using System.Collections.Generic;

public class FarmShop : MonoBehaviour, IDropOffHandler, INPCPickUpHandler
{
    [Header("Production Variables")]    
    //UI Prefabs
    public GameObject dropOffRequirementUIPrefab;
    [SerializeField] private ShopInventory inventory;
    private ItemTypes shopItemTypes;
    private ItemCosts priceList;
    [SerializeField] float priceMultiplier = 1.0f;
    private DropOffRequirementUI dropOffUI;
    private int maxSyrupRequired = 100;
    private int currentSyrupCount = 0; 
    public string displaySyrupCount = "0";
    [SerializeField] private int minCustomerItems = 0;
    [SerializeField] private int maxCustomerItems = 10;
    private int totalItemsInShop = 0; //tally of number of items in shop - required for npc pickup to work without infinite loops
    private ItemTypes.Types itemToBuyKey;
    private string itemToBuyStr;
    [SerializeField] private Wallet playerWallet;
    public Dictionary<ItemTypes.Types, float> costDict = new Dictionary<ItemTypes.Types, float>();
    [Header("Item Requirement Icon")]
    public Sprite sapIcon; //Assign in the Inspector

    private Canvas mainCanvas;

    private void Start()
    {
        shopItemTypes = new ItemTypes();
        priceList = new ItemCosts();

        //hard code cost list for bug workaround
        priceList.costDict[ItemTypes.Types.Sap] = 1f;
        priceList.costDict[ItemTypes.Types.Sap] = 1f;
        priceList.costDict[ItemTypes.Types.SyrupUnfiltered] = 6f;
        priceList.costDict[ItemTypes.Types.SyrupFiltered] = 12f;
        priceList.costDict[ItemTypes.Types.SyrupBottleUnfiltered] = 12f;
        priceList.costDict[ItemTypes.Types.SyrupBottleFiltered] = 24f;
        priceList.costDict[ItemTypes.Types.TaffyTray] = 8f;
        priceList.costDict[ItemTypes.Types.TaffyBox] = 10f;
        priceList.costDict[ItemTypes.Types.IceCreamBucket] = 25f;
        //costDict[ItemTypes.Types.SnowCandySingle] = 1f;
        //costDict[ItemTypes.Types.SnowCandyBox] = 1f;

        //init stock
        UnityEngine.Debug.Log("are we here stock check");
        foreach (KeyValuePair<ItemTypes.Types, float> entry in priceList.costDict)
        {
            // Store the key in the itemKeys list
            //itemKeys.Add(entry.Key);
            UnityEngine.Debug.Log(entry.Key);
        }

        foreach (KeyValuePair<ItemTypes.Types, float> entry in priceList.costDict)
        {
            UnityEngine.Debug.Log("are we here");
            UnityEngine.Debug.Log($"Item: {entry.Key}, Cost: {entry.Value}");
        }

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
            //ItemTypes.Types itemType in System.Enum.GetValues(typeof(ItemTypes.Types))

            itemToBuyKey = shopItemTypes.GetRandomEnumKey();
            itemToBuyStr = itemToBuyKey.ToString();
            //Types enumFromValue = (Types)value;

            UnityEngine.Debug.Log($"NPC wants a {itemToBuyKey}!");


            //if it's not in stock check for a different item
            if (inventory.currentStockDict[itemToBuyKey] == 0)
            {
                UnityEngine.Debug.Log($"NPC wants a {itemToBuyStr}: none in stock!");
                itemToBuyKey = shopItemTypes.GetRandomEnumKey();
                UnityEngine.Debug.Log($"NPC will try a {itemToBuyStr} instead if it's in stock but will give up if it's not and move on to their next wanted item!");
            }
            
            //if there is stock buy one, but if not we only check once for an alternate and then keep moving, odd but that's the logic for now
            if (inventory.currentStockDict[itemToBuyKey] > 0)
            {                
                inventory.currentStockDict[itemToBuyKey] -= 1;
                float payment = priceList.costDict[itemToBuyKey] * priceMultiplier;
                UnityEngine.Debug.Log($"NPC buys a {itemToBuyKey} for {payment} beaver bucks!");
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
