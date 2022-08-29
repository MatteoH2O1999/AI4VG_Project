using UnityEngine;

public class ReachedCoverTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        Vector3 cover = agent.GetCover().Value;
        Vector3 currentPosition = new(agent.transform.position.x, cover.y, agent.transform.position.z);
        return Vector3.Distance(cover, currentPosition) < agent.GetDelegates().GetCoverMaster().distanceBetweenPoints / 5.0f;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}