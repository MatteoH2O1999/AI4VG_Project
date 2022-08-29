using UnityEngine;

public class ExitingCoverState : FSMState
{
    private Vector3? exitCoverPoint;
    public override void OnEntry(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        Vector3 position = agent.transform.position;
        Vector3 standingGun = new(position.x, position.y + agent.gunHeightComparedToPivot, position.z);
        Vector3 enemyPosition = delegates.enemy.transform.position;
        enemyPosition.y = standingGun.y;
        Vector3 direction = enemyPosition - standingGun;
        if(Physics.Raycast(standingGun, direction, out RaycastHit hit))
        {
            if(hit.collider == delegates.enemy)
            {
                this.exitCoverPoint = null;
                return;
            }
        }
        Vector3[] possibleCovers = delegates.GetCoverMaster().GetReservedPoints(agent.gameObject);
        foreach (Vector3 possibleCover in possibleCovers)
        {
            standingGun = new(possibleCover.x, possibleCover.y + agent.gunHeightComparedToPivot, possibleCover.z);
            direction = enemyPosition - standingGun;
            if(Physics.Raycast(standingGun, direction, out RaycastHit possibleHit))
            {
                if(possibleHit.collider == delegates.enemy)
                {
                    this.exitCoverPoint = possibleCover;
                    return;
                }
            }
        }
        this.ExitCover(agent);
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

    private void ExitCover(CoverAgent agent)
    {
        CoverAgentDelegates delegates = agent.GetDelegates();
        delegates.Stand();
        if (this.exitCoverPoint.HasValue)
        {
            Vector3 coverPoint = this.exitCoverPoint.Value;
            delegates.MoveTo(coverPoint);
        }
    }
}