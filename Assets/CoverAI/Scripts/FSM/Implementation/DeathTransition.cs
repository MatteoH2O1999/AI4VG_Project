public class DeathTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        return agent.GetComponent<CoverAgentDelegates>().IsDead();
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}