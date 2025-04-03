using UnityEngine;
using UnityEngine.UI;
using TMPro; // Uncommented for TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Wallet playerWallet;
    public int defaultMoney = 100; // Fixed typo from "defaultrMoney"

    // UI Text Component (Set this in Inspector)
    [SerializeField] private TMP_Text moneyText; // Changed to TMP_Text for TextMeshPro

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerWallet.Money = defaultMoney;
        UpdateMoneyUI();
    }

    public float GetPlayerMoney()
    {
        return playerWallet.Money;
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"${GetPlayerMoney()}"; // Cleaner formatting
        }
    }

    public bool SpendMoney(int amount)
    {
        if (playerWallet.Money >= amount)
        {
            playerWallet.Money -= amount; // Deduct money
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    public void ReceiveMoney(int amount)
    {
        playerWallet.Money += amount; // Add money
        UpdateMoneyUI();
    }
}
