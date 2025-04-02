using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerMoney = 200; // Starting money

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;

            //UI guys take a look here and update this to match our system for the HUD
           // UIManager.Instance.UpdateMoneyDisplay(playerMoney); // Update UI
            return true;
        }
        return false;
    }
}