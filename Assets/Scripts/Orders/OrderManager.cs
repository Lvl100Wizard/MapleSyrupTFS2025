using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    public OrderData[] orderQueue;
    private int currentOrderIndex = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public OrderData GetCurrentOrder()
    {
        if (currentOrderIndex < orderQueue.Length)
            return orderQueue[currentOrderIndex];
        return null;
    }

    public void CompleteOrder()
    {
        if (currentOrderIndex < orderQueue.Length)
        {
            OrderData currentOrder = orderQueue[currentOrderIndex];
            float reward = GetOrderReward(currentOrder.name); // Use ScriptableObject name

            // Grant money using the Wallet inside GameManager
            GameManager.Instance.ReceiveMoney((int)reward);  // Cast to int as your ReceiveMoney method takes an int

            Debug.Log($"Order {currentOrder.name} completed! Player earned ${reward}");

            currentOrderIndex++;

            if (currentOrderIndex < orderQueue.Length)
                Debug.Log($"Next Order: {orderQueue[currentOrderIndex].name} started.");
            else
            {
                Debug.Log("All orders completed!");
                FadeManager.instance.FadeToScene("WinScene");
            }
        }
    }

    private float GetOrderReward(string orderName)
    {
        switch (orderName)
        {
            case "Order_1": return 50f;
            case "Order_2": return 100f;
            case "Order_3": return 150f;
            case "Order_4": return 200f;
            case "Order_5": return 250f;
            case "Order_6": return 300f;
            case "Order_7": return 400f;
            case "Order_8": return 500f;
            case "Order_9": return 600f;
            case "Order_10": return 1000f;

            default:
                Debug.LogWarning($"Order {orderName} has no predefined reward!");
                return 0f;
        }
    }
}
