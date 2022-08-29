public class NotInCoverFSM : FSM
{
    public NotInCoverFSM()
    {
        FSMState movingTowardsPlayer = new MoveTowardsPlayerState();
        FSMState rangedAttackStanding = new RangedAttackStandingState();
        FSMTransition notInSightTransition = new TargetNotInSightTransition();
        FSMTransition inSightTransition = new TargetInSightTransition();
        this.currentState = movingTowardsPlayer;
        movingTowardsPlayer.AddLink(inSightTransition, rangedAttackStanding);
        rangedAttackStanding.AddLink(notInSightTransition, movingTowardsPlayer);
    }

    public override void OnExit(CoverAgent agent)
    {
    }
}