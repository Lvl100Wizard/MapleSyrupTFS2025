public class ItemTypes
{

    #region variables
    public enum Types
    {
        sap,
        taffee,
        syrup,
        bacon,
    };

    public Types type;
    #endregion

    #region Getters and Setters
    public void SetItemType(ItemTypes.Types itemType)
    {
        type = itemType;
    }

    public Types GetItemType()
    {
        return type;
    }
    #endregion

}