public class DeathTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        return agent.GetDelegates().IsDead();
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}