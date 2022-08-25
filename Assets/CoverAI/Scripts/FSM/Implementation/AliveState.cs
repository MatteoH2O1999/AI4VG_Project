public class AliveState : FSM
{
    public AliveState()
    {
        FSMState idleState = new IdleState();
        this.currentState = idleState;
        FSMTransition farTransition = new FarTransition();
        FSMState moveTowardsPlayerState = new MoveTowardsPlayerState();
        idleState.AddLink(farTransition, moveTowardsPlayerState);
    }

    public override void OnEntry(CoverAgent agent)
    {
    }

    public override void OnExit(CoverAgent agent)
    {
    }

    public override void OnInterrupt(CoverAgent agent)
    {
    }
}