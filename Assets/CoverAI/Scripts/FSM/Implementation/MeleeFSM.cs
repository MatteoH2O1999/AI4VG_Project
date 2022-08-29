public class MeleeFSM : FSM
{
    public MeleeFSM()
    {
        FSMState chargeState = new ChargeState();
        FSMState meleeAttackState = new MeleeAttackState();
        FSMTransition chargeTransition = new ChargeTransition();
        FSMTransition meleeAttackTransition = new MeleeAttackTransition();
        this.currentState = chargeState;
        chargeState.AddLink(meleeAttackTransition, meleeAttackState);
        meleeAttackState.AddLink(chargeTransition, chargeState);
    }

    public override void OnExit(CoverAgent agent)
    {
    }
}