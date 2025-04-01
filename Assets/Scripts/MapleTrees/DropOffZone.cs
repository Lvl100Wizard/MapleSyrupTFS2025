using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered
        {
            IDropOffHandler dropOffHandler = GetComponentInParent<IDropOffHandler>();

            if (dropOffHandler != null)
            {
                dropOffHandler.HandleDropOff(other.GetComponent<PlayerObjects>());
            }
        }
    }
}
