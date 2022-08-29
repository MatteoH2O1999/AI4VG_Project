public class MeleeTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        return delegates.DistanceTo(delegates.enemy) < agent.meleeChargeRange;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}