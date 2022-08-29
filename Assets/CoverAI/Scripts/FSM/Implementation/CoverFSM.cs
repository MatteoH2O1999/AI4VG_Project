public class CoverFSM : FSM
{ 
    public CoverFSM()
    {
        FSMState aliveState = new AliveState();
        this.currentState = aliveState;
        aliveState.AddLink(new DeathTransition(), new DeadState());
    }

    public override void OnExit(CoverAgent agent)
    {
    }
}