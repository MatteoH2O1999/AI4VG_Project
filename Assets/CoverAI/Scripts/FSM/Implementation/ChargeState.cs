public class ChargeState : FSMState
{
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.Charge();
        delegates.MoveTo(delegates.enemy.transform.position);
    }

    public override void OnExit(CoverAgent agent)
    {
    }

    public override void OnInterrupt(CoverAgent agent)
    {
    }

    public override void OnStay(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.MoveTo(delegates.enemy.transform.position);
    }
}