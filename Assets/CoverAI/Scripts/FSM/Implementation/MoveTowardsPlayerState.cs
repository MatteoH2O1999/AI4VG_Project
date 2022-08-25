public class MoveTowardsPlayerState : FSMState
{
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetComponent<CoverAgentDelegates>();
        delegates.Stand();
        delegates.MoveTo(delegates.enemy.transform.position);
    }

    public override void OnExit(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetComponent<CoverAgentDelegates>();
        delegates.MoveTo(agent.transform.position);
    }

    public override void OnInterrupt(CoverAgent agent)
    {
    }

    public override void OnStay(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetComponent<CoverAgentDelegates>();
        delegates.MoveTo(delegates.enemy.transform.position);
    }
}