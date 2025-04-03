using UnityEngine;

[System.Serializable]
public class OrderRequirement
{
    public string itemTag;
    public int requiredAmount;
    public Sprite icon;
}


[CreateAssetMenu(fileName = "NewOrder", menuName = "Order System/Complex Order")]
public class OrderData : ScriptableObject
{
    public OrderRequirement[] requiredItems;
}
