using UnityEngine;

public class PickUpZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered
        {
            IPickUpHandler pickUpHandler = GetComponentInParent<IPickUpHandler>();

            if (pickUpHandler != null)
            {
                pickUpHandler.HandlePickup(other.GetComponent<PlayerObjects>());
            }

        }
    }
}

