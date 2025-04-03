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
        if (currentOrderIndex < orderQueue.Length - 1)
        {
            currentOrderIndex++;
            Debug.Log($"Next Order: Order {currentOrderIndex + 1} started.");
        }
        else
        {
            Debug.Log("All orders completed!");
            FadeManager.instance.FadeToScene("WinScene");
        }
    }
}
