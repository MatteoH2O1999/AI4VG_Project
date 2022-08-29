public class MeleeAttackState : FSMState
{
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.MeleeAttack(delegates.enemy);
    }

    public override void OnExit(CoverAgent agent)
    {
    }

    public override void OnInterrupt(CoverAgent agent)
    {
    }

    public override void OnStay(CoverAgent agent)
    {
    }
}