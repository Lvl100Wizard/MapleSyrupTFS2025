using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Wallet", order = 1)]
public class ShopInventory : AbstractSOContainer
{

    #region variables
    private GenericItem sap;
    private GenericItem syrup;
    private GenericItem taffee;
    private GenericItem bacon;

    #endregion

    #region Built-in Functions
    public void Start()
    {
        InitShop();
    }
    #endregion

    #region Custom Functions
    public void InitShop()
    {
        sap.Type = ItemTypes.Types.sap;
        syrup.Type = ItemTypes.Types.syrup;
        taffee.Type = ItemTypes.Types.taffee;
        bacon.Type = ItemTypes.Types.bacon;
    }
    #endregion
}