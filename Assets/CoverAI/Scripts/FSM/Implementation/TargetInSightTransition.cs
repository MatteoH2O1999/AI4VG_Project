using System;
using UnityEngine;

public class TargetInSightTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        Vector3 currentPosition = agent.transform.position + new Vector3(0, agent.gunHeightComparedToPivot, 0);
        Vector3 enemyPosition = delegates.GetCoverMaster().GetPeak(delegates.enemy);
        Vector3 direction = enemyPosition - currentPosition;
        if (Physics.Raycast(currentPosition, direction, out RaycastHit hit))
        {
            if (hit.collider.gameObject == agent.GetDelegates().enemy)
                return true;
        }
        return false;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}