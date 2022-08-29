public class RangedAttackInCoverState : FSMState
{
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.RangedAttack(delegates.enemy);
    }

    public override void OnExit(CoverAgent agent)
    {
    }

    public override void OnInterrupt(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.GetCoverMaster().FreeCover(agent.gameObject);
        agent.SetCover(null);
    }

    public override void OnStay(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.RangedAttack(delegates.enemy);
    }
}