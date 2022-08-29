using UnityEngine;
using UnityEngine.AI;

public class NavmeshDelegates : CoverDelegates
{
    public int maxMapHeight;
    public float maxAgentHeight;
    public override Vector3 GetPositionOnTerrain(Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(position.x, this.maxMapHeight, position.z), Vector3.down);
        if (hits.Length == 0)
            return Vector3.zero;
        Vector3 toReturn = hits[0].point;
        foreach (RaycastHit hit in hits)
        {
            Vector3 point = hit.point;
            if(Vector3.Distance(toReturn, position) > Vector3.Distance(point, position))
                toReturn = point;
        }
        return toReturn;
    }

    public override bool IsOnTerrain(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, this.maxAgentHeight * 2, NavMesh.AllAreas))
            return false;
        Vector3 navMeshPoint = hit.position;
        Vector3 verticalProjection = new Vector3(position.x, navMeshPoint.y, position.z);
        return Vector3.Distance(navMeshPoint, verticalProjection) <= 0.1f;
    }

    public override bool IsReachable(Vector3 startPosition, Vector3 endPosition)
    {
        startPosition = this.GetPositionOnTerrain(startPosition);
        endPosition = this.GetPositionOnTerrain(endPosition);
        NavMeshPath path = new NavMeshPath();
        if(NavMesh.CalculatePath(startPosition, endPosition, NavMesh.AllAreas, path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }
}
