using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShopInventory", order = 1)]
public class ShopInventory : AbstractSOContainer
{
    public Dictionary<ItemTypes.Types, int> currentStockDict = new Dictionary<ItemTypes.Types, int>();

    #region variables
    private GenericItem sap;
    private GenericItem syrupUnfiltered;
    private GenericItem syrupFiltered;
    private GenericItem syrupBottleUnfiltered;
    private GenericItem syrupBottleFiltered;
    private GenericItem taffyTray;
    private GenericItem taffyBox;
    private GenericItem iceCreamBucket;
    //private GenericItem snowCandySingle;
    //private GenericItem snowCandyBox;
    #endregion

    #region Built-in Functions
    public void Start()
    {
        InitShopItems();
        InitShopStock();
    }
    #endregion

    #region Custom Functions
    public void InitShopItems()
    {
        sap.Type = ItemTypes.Types.Sap;
        syrupUnfiltered.Type = ItemTypes.Types.SyrupUnfiltered;
        syrupFiltered.Type = ItemTypes.Types.SyrupFiltered;
        syrupBottleUnfiltered.Type = ItemTypes.Types.SyrupBottleUnfiltered;
        syrupBottleFiltered.Type = ItemTypes.Types.SyrupBottleFiltered;
        taffyTray.Type = ItemTypes.Types.TaffyTray;
        taffyBox.Type = ItemTypes.Types.TaffyBox;
        iceCreamBucket.Type = ItemTypes.Types.IceCreamBucket;
        //snowCandySingle.Type = ItemTypes.Types.SnowCandySingle;
        //snowCandyBox.Type = ItemTypes.Types.SnowCandyBox;
    }

    public void InitShopStock()
    {
        currentStockDict[ItemTypes.Types.Sap] = 0;
        currentStockDict[ItemTypes.Types.SyrupUnfiltered] = 0;
        currentStockDict[ItemTypes.Types.SyrupFiltered] = 0;
        currentStockDict[ItemTypes.Types.SyrupBottleUnfiltered] = 0;
        currentStockDict[ItemTypes.Types.SyrupBottleFiltered] = 0;
        currentStockDict[ItemTypes.Types.TaffyTray] = 0;
        currentStockDict[ItemTypes.Types.TaffyBox] = 0;
        currentStockDict[ItemTypes.Types.IceCreamBucket] = 0;
        //currentStockDict[ItemTypes.Types.SnowCandySingle] = 0;
        //currentStockDict[ItemTypes.Types.SnowCandyBox] = 0;
    }
    #endregion
}