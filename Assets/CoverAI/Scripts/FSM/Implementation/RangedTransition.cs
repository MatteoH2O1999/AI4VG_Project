public class RangedTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetComponent<CoverAgentDelegates>();
        float distance = delegates.DistanceTo(delegates.enemy);
        return distance > agent.meleeChargeRange && distance < agent.rangedAttackRange;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}