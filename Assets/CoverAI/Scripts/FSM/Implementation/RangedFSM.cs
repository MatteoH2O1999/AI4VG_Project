public class RangedFSM : FSM
{
    private FSMState resetState;
    public RangedFSM()
    {
        FSMState notInCover = new NotInCoverFSM();
        FSMState inCover = new InCoverFSM();
        FSMState goingToCover = new GoingToCoverState();
        FSMTransition goingToCoverTransition = new MoveToCoverTransition();
        FSMTransition leaveCoverTransition = new LeaveCoverTransition();
        FSMTransition coverReachedTransition = new ReachedCoverTransition();
        this.currentState = notInCover;
        this.resetState = notInCover;
        notInCover.AddLink(goingToCoverTransition, goingToCover);
        goingToCover.AddLink(coverReachedTransition, inCover);
        inCover.AddLink(leaveCoverTransition, notInCover);
    }

    public override void OnExit(CoverAgent agent)
    {
        this.currentState = this.resetState;
        if (agent.GetCover().HasValue)
        {
            CoverAgentDelegates delegates = agent.GetDelegates();
            delegates.GetCoverMaster().FreeCover(agent.gameObject);
            agent.SetCover(null);
        }
    }
}