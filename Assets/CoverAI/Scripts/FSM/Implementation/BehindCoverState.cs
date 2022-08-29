using UnityEngine;

public class BehindCoverState : FSMState
{
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        Vector3 currentPosition = agent.transform.position;
        Vector3 gunPosition = new(currentPosition.x, currentPosition.y + agent.gunHeightComparedToPivot, currentPosition.z);
        Vector3 enemyPosition = delegates.enemy.transform.position;
        enemyPosition.y = gunPosition.y;
        Vector3 direction = enemyPosition - gunPosition;
        if (Physics.Raycast(gunPosition, direction, out RaycastHit hit))
        {
            if (hit.collider.gameObject == delegates.enemy)
                delegates.Crouch();
        }
        else
            delegates.Stand();
    }

    public override void OnExit(CoverAgent agent)
    {
    }

    public override void OnInterrupt(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.GetCoverMaster().FreeCover(agent.gameObject);
        agent.SetCover(null);
    }

    public override void OnStay(CoverAgent agent)
    {
    }
}