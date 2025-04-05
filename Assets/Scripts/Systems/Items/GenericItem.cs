public class GenericItem
{

    #region variables
    private ItemTypes.Types type;
    private int numItem = 0;
    #endregion

    #region properties
    public ItemTypes.Types Type
    {
        get { return type; }
        set { type = value; }
    }

    public int NumItem 
    { 
        get { return numItem; } 
        set { numItem = value; } 
    }
    #endregion

}
