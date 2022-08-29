using UnityEngine;

public class InCoverFSM : FSM
{
    public InCoverFSM()
    {
        FSMState behindCoverState = new BehindCoverState();
        FSMState exitingCoverState = new ExitingCoverState();
        FSMState attackingState = new RangedAttackInCoverState();
        FSMState moveToCoverState = new GoingToCoverState();
        FSMTransition targetInSightTransition = new TargetInSightTransition();
        FSMTransition coverReachedTransition = new ReachedCoverTransition();
        FSMTransition startAttackTransition = new BeginAttackTransition();
        FSMTransition stopAttackTransition = new StopAttackingTransition();
        this.currentState = behindCoverState;
        behindCoverState.AddLink(startAttackTransition, exitingCoverState);
        exitingCoverState.AddLink(targetInSightTransition, attackingState);
        attackingState.AddLink(stopAttackTransition, moveToCoverState);
        moveToCoverState.AddLink(coverReachedTransition, behindCoverState);
    }

    public override void OnExit(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.GetCoverMaster().FreeCover(agent.gameObject);
        agent.SetCover(null);
    }
}