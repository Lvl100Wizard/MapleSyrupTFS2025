
using UnityEngine;

public class UnlockZone : MonoBehaviour
{

    [SerializeField] public int price = 100;
     public StructureBuilder structureToUnlock;
    private bool isUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUnlocked)
        {
            if (GameManager.Instance.SpendMoney(price)) //check if player has balance for unlock
            {
                UnlockFeature();
            }
            else
            {
                Debug.Log("not enough money");
            }
        }
    }

    private void UnlockFeature()
    {
        isUnlocked = true;
        structureToUnlock.StartBuild();
        gameObject.SetActive(false); //disable trigger zone
    }
}
