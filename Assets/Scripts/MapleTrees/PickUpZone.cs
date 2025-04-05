using UnityEngine;

public class PickUpZone : MonoBehaviour
{
    [SerializeField] private AgentTypes.Types agent = AgentTypes.Types.Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(AgentTypes.GetAgentTypeStringName(agent))) // Check if the correct agent type entered - defaults to player
        {
            IPickUpHandler pickUpHandler = GetComponentInParent<IPickUpHandler>();

            if (pickUpHandler != null)
            {
                pickUpHandler.HandlePickup(other.GetComponent<PlayerObjects>());
            }

        }
    }
    
}

