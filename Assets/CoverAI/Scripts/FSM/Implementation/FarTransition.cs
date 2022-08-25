public class FarTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetComponent<CoverAgentDelegates>();
        return delegates.DistanceTo(delegates.enemy) > agent.rangedAttackRange;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}