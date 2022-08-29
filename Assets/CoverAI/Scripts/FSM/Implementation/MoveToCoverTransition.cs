using UnityEngine;

public class MoveToCoverTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        Vector3? cover = delegates.GetCoverMaster().GetCover(agent.gameObject, delegates.enemy, agent.gunHeightComparedToPivot, agent.gunCrouchHeightComparedToPivot, agent.pivotHeight);
        if(cover.HasValue)
        {
            agent.SetCover(cover.Value);
            return true;
        }
        return false;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}