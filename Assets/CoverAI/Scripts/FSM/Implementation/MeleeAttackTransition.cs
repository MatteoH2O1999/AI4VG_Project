public class MeleeAttackTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        return delegates.DistanceTo(delegates.enemy) < agent.meleeAttackRange;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}