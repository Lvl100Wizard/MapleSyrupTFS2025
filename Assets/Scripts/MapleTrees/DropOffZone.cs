using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    [SerializeField] private AgentTypes.Types agent = AgentTypes.Types.Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(AgentTypes.GetAgentTypeStringName(agent))) // Check if the correct agent type entered - defaults to player
        {
            UnityEngine.Debug.Log($"handle dropoff for {AgentTypes.GetAgentTypeStringName(agent)}");
            IDropOffHandler dropOffHandler = GetComponentInParent<IDropOffHandler>();

            if (dropOffHandler != null)
            {
                dropOffHandler.HandleDropOff(other.GetComponent<PlayerObjects>());
            }
        }
    }
}
