using System;

public class AgentTypes
{

    #region variables
    public enum Types
    {
        Player,
        NpcIndividual,
        NpcBusiness,
        NpcFederalReserve,
    };

    public Types type;
    #endregion

    #region Getters and Setters
    public void SetAgentType(AgentTypes.Types agentType)
    {
        type = agentType;
    }

    public Types GetAgentType()
    {
        return type;
    }

    public static String GetAgentTypeStringName(Types agentType)
    {
        return Enum.GetName(typeof(Types), agentType);
    }
    #endregion

}