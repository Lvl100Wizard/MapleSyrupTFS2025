public class ItemTypes
{

    #region variables
    public enum Types //values here must match existing unity tags
    {
        Sap,
        Taffee,
        Syrup,
        Bacon,
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