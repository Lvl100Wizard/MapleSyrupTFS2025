using UnityEngine;

public class MapleTree : MonoBehaviour
{
    public bool canTap = true;
    public float tapCooldown = 10.0f;
    public SapTimerUI timerSlider; 

    private void Start()
    {
        if (timerSlider != null)
        {
            if (canTap)
            {
                timerSlider.SetSliderValue(1f);
            }
            else
            {
                timerSlider.SetSliderValue(0f);
                timerSlider.StartCooldown(tapCooldown, () => EndCooldown());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTap)
        {
            Debug.Log("Harvesting Sap!");
            canTap = false;
            
            timerSlider.StartCooldown(tapCooldown, () => EndCooldown());
        }
        else if (!canTap)
        {
            Debug.Log("Not ready to harvest!");
        }
    }

    private void EndCooldown()
    {
        canTap = true;
        Debug.Log("Ready to harvest!");
    }
}
