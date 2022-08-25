public abstract class FSM : FSMState
{
    protected FSMState currentState;

    public FSM()
    {
        this.currentState = null;
    }

    public FSM(FSMState startingState)
    {
        this.currentState = startingState;
    }

    public void UpdateFSM(CoverAgent agent)
    {
        FSMState nextState;
        FSMTransition transition;
        (nextState, transition) = this.currentState.NextState(agent);
        if (nextState != null)
        {
            if (this.currentState is FSM fsm)
            {
                fsm.OnInterrupt(agent); // If the state is a nested FSM, signal the interrupt.
            }
            this.currentState.OnExit(agent);
            transition.TransitionActions(agent);
            this.currentState = nextState;
            this.currentState.OnEntry(agent);
        }
        else
        {
            this.currentState.OnStay(agent);
        }
    }

    public override void OnStay(CoverAgent agent)
    {
        this.UpdateFSM(agent);
    }
}