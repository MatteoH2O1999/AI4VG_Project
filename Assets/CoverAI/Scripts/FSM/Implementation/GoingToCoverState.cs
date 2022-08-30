using UnityEngine;
using UnityEngine.AI;

public class GoingToCoverState : FSMState
{
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        Vector3 cover = agent.GetCover().Value;
        Vector3 currentPosition = new(agent.transform.position.x, cover.y, agent.transform.position.z);
        if (Vector3.Distance(cover, currentPosition) > agent.GetDelegates().GetCoverMaster().distanceBetweenPoints / 5.0f)
        {
            delegates.Stand();
        }
        delegates.MoveTo(agent.GetCover().Value);
    }

    public override void OnExit(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.MoveTo(agent.transform.position);
    }

    public override void OnInterrupt(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.GetCoverMaster().FreeCover(agent.gameObject);
        agent.SetCover(null);
    }

    public override void OnStay(CoverAgent agent)
    {
    }
}