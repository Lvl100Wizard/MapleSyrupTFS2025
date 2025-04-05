using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    [SerializeField] private AgentTypes.Types agent = AgentTypes.Types.Player;

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (other.CompareTag(AgentTypes.GetAgentTypeStringName(agent))) // Check if the correct agent type entered - defaults to player
        {
            UnityEngine.Debug.Log($"handle dropoff for {AgentTypes.GetAgentTypeStringName(agent)}");
            IDropOffHandler dropOffHandler = GetComponentInParent<IDropOffHandler>();

            if (tag == AgentTypes.GetAgentTypeStringName(AgentTypes.Types.Player))
            {
                if (dropOffHandler != null)
                {
                    dropOffHandler.HandleDropOff(other.GetComponent<PlayerObjects>());
                }
            }/*
            else if (tag == AgentTypes.GetAgentTypeStringName(AgentTypes.Types.NpcIndividual))
            {
                if (dropOffHandler != null)
                {
                    dropOffHandler.HandleDropOff();
                }
            }*/
        }
        
    }
}
