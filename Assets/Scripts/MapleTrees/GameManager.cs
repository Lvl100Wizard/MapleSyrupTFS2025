using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    
    
    
    [SerializeField] private Wallet playerWallet;


    public int defaultrMoney = 100; // Starting money

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerWallet.Money = defaultrMoney;
    }
    public bool SpendMoney(int amount)
    {
        if (playerWallet.Money >= amount)
        {
            playerWallet.GiveMoney(amount);

            //UI guys take a look here and update this to match our system for the HUD
           // UIManager.Instance.UpdateMoneyDisplay(playerMoney); // Update UI
            return true;
        }
        return false;
    }
}