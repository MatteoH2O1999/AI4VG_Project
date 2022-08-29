public class AliveState : FSM
{
    public AliveState()
    {
        FSMState idleState = new IdleState();
        FSMState moveTowardsPlayerState = new MoveTowardsPlayerState();
        FSMState rangedState = new RangedFSM();
        FSMState meleeState = new MeleeFSM();
        FSMTransition farTransition = new FarTransition();
        FSMTransition rangedTransition = new RangedTransition();
        FSMTransition meleeTransition = new MeleeTransition();
        this.currentState = idleState;
        idleState.AddLink(farTransition, moveTowardsPlayerState);
        idleState.AddLink(rangedTransition, rangedState);
        idleState.AddLink(meleeTransition, meleeState);
        moveTowardsPlayerState.AddLink(rangedTransition, rangedState);
        moveTowardsPlayerState.AddLink(meleeTransition, meleeState);
        rangedState.AddLink(farTransition, moveTowardsPlayerState);
        rangedState.AddLink(meleeTransition, meleeState);
        meleeState.AddLink(farTransition, moveTowardsPlayerState);
        meleeState.AddLink(rangedTransition, rangedState);
    }

    public override void OnExit(CoverAgent agent)
    {
    }
}