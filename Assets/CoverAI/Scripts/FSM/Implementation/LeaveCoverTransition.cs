using UnityEngine;

public class LeaveCoverTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        Vector3[] covers = delegates.GetCoverMaster().GetReservedPoints(agent.gameObject);
        Vector3 enemyPosition = delegates.GetCoverMaster().GetPeak(delegates.enemy);
        Vector3 agentCrouchPosition = agent.transform.position + new Vector3(0, agent.gunCrouchHeightComparedToPivot, 0);
        if (Physics.Raycast(agentCrouchPosition, enemyPosition - agentCrouchPosition, out RaycastHit coverHit))
        {
            if (coverHit.collider.gameObject == delegates.enemy)
                return true;
        }
        else
            return true;
        foreach (Vector3 cover in covers)
        {
            Vector3 possiblePosition = new(cover.x, agent.transform.position.y + agent.gunHeightComparedToPivot, cover.z);
            Vector3 direction = enemyPosition - possiblePosition;
            if (Physics.Raycast(possiblePosition, direction, out RaycastHit hit))
            {
                if(hit.collider.gameObject == delegates.enemy)
                    return false;
            }
        }
        return true;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}