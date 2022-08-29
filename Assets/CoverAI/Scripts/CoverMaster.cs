using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CoverDelegates))]
public class CoverMaster : MonoBehaviour
{

    public float distanceBetweenPoints = 1.0f;
    public float distanceBetweenCachePoints = 0.5f;
    public List<Vector3> spawns;

    private readonly HashSet<CoverPoint> coverPoints = new();
    private readonly Dictionary<Vector3, CoverPoint> coverPointsMap = new();
    private readonly Dictionary<Vector3, List<Vector3>> cache = new();
    private const float distanceForPointEvaluation = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        this.UpdatePoints();
    }

    // Generate cover points
    void UpdatePoints()
    {
        HashSet<Vector3> coverPositions = new();
        this.coverPointsMap.Clear();
        this.cache.Clear();

        CoverDelegates delegates = GetComponent<CoverDelegates>();

        List<Vector3> borderPoints = new();
        foreach (var point in this.spawns)
        {
            float newX = Mathf.Round(point.x / this.distanceBetweenPoints) * this.distanceBetweenPoints;
            float newZ = Mathf.Round(point.z / this.distanceBetweenPoints) * this.distanceBetweenPoints;
            float newY = delegates.GetPositionOnTerrain(new Vector3(newX, point.y, newZ)).y;
            Vector3 gridPoint = new(newX, newY, newZ);
            if(!borderPoints.Contains(gridPoint))
                borderPoints.Add(gridPoint);
        }

        while(borderPoints.Count > 0)
        {
            Vector3 borderPoint = borderPoints[0];
            if(!delegates.IsOnTerrain(borderPoint))
            {
                borderPoints.RemoveAt(0);
                continue;
            }
            bool reachable = false;
            foreach (var spawnPoint in this.spawns)
            {
                if(delegates.IsReachable(borderPoint, spawnPoint))
                {
                    reachable = true;
                    break;
                }
            }
            if (!reachable)
            {
                borderPoints.RemoveAt(0);
                continue;
            }
            if (!coverPositions.Contains(this.Gridify(borderPoint, this.distanceBetweenPoints)))
            {
                coverPositions.Add(this.Gridify(borderPoint, this.distanceBetweenPoints));
                CoverPoint p = new CoverPoint(borderPoint);
                this.coverPoints.Add(p);
                this.coverPointsMap.Add(p.getPosition(), p);
                float x = borderPoint.x;
                float y = borderPoint.y;
                float z = borderPoint.z;
                borderPoints.Add(new Vector3(x + this.distanceBetweenPoints, delegates.GetPositionOnTerrain(new Vector3(x + this.distanceBetweenPoints, y, z)).y, z));
                borderPoints.Add(new Vector3(x - this.distanceBetweenPoints, delegates.GetPositionOnTerrain(new Vector3(x - this.distanceBetweenPoints, y, z)).y, z));
                borderPoints.Add(new Vector3(x, delegates.GetPositionOnTerrain(new Vector3(x, y, z + this.distanceBetweenPoints)).y, z + this.distanceBetweenPoints));
                borderPoints.Add(new Vector3(x, delegates.GetPositionOnTerrain(new Vector3(x, y, z - this.distanceBetweenPoints)).y, z - this.distanceBetweenPoints));
            }
            borderPoints.RemoveAt(0);
        }
    }

    public Vector3 GetPeak(GameObject enemy)
    {
        CoverDelegates delegates = GetComponent<CoverDelegates>();
        Vector3 enemyPosition = enemy.transform.position;
        // TODO: Raycast in an area
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(enemyPosition.x, delegates.GetPositionOnTerrain(enemyPosition).y + 1, enemyPosition.z), Vector3.down);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == enemy)
                return hit.point;
        }
        return enemy.transform.position;
    }

    public Vector3? GetCover(GameObject agent, GameObject enemy, float gunHeightComparedToPivot, float gunCrouchHeightComparedToPivot, float pivotHeightComparedToTerrain)
    {

        Vector3 enemyGridPosition = this.Gridify(enemy.transform.position, this.distanceBetweenCachePoints);
        Vector3 agentGridPosition = this.Gridify(agent.transform.position, this.distanceBetweenCachePoints);
        if (this.cache.ContainsKey(enemyGridPosition) && this.cache[enemyGridPosition].Contains(agentGridPosition))
            return null;

        CoverAgent coverAgent = agent.GetComponent<CoverAgent>();
        CoverAgentDelegates coverAgentDelegates = coverAgent.GetComponent<CoverAgentDelegates>();
        if (coverAgent == null || coverAgentDelegates == null)
            throw new Exception(); // TODO: Better exception
        float maxCoverDistanceFromPlayer = coverAgent.rangedAttackRange;
        float minCoverDistanceFromPlayer = coverAgent.meleeChargeRange;

        Vector3 additiveStandingHeight = new(0, pivotHeightComparedToTerrain + gunHeightComparedToPivot, 0);
        Vector3 additiveCrouchHeigh = new(0, pivotHeightComparedToTerrain + gunCrouchHeightComparedToPivot, 0);
        Vector3 enemyPeak = this.GetPeak(enemy);
        Dictionary<Vector3, int> possiblePoints = new();
        Vector3 nearestPoint = this.GetNearestPoint(agent.transform.position);

        List<Vector3> toSee = new()
        {
            nearestPoint
        };
        HashSet<Vector3> seen = new();

        while (toSee.Count > 0)
        {
            Vector3 possiblePoint = toSee[0];
            float distance = coverAgentDelegates.DistanceBetween(possiblePoint, enemy.transform.position);
            if(distance > maxCoverDistanceFromPlayer || distance < minCoverDistanceFromPlayer)
            {
                toSee.Remove(possiblePoint);
                continue;
            }

            Vector3 neighbour1 = this.GetNearestPoint(possiblePoint + new Vector3(this.distanceBetweenPoints, 0, 0));
            Vector3 neighbour2 = this.GetNearestPoint(possiblePoint + new Vector3(-this.distanceBetweenPoints, 0, 0));
            Vector3 neighbour3 = this.GetNearestPoint(possiblePoint + new Vector3(0, 0, this.distanceBetweenPoints));
            Vector3 neighbour4 = this.GetNearestPoint(possiblePoint + new Vector3(0, 0, -this.distanceBetweenPoints));

            if (!this.coverPointsMap[possiblePoint].isOccupied() && coverAgentDelegates.DistanceBetween(possiblePoint, agent.transform.position) <= coverAgentDelegates.DistanceBetween(agent.transform.position, enemy.transform.position))
            {
                if(this.CanHit(possiblePoint + additiveStandingHeight, enemyPeak, enemy))
                {
                    if(!this.CanHit(possiblePoint + additiveCrouchHeigh, enemyPeak, enemy))
                    {
                        possiblePoints.Add(possiblePoint, 1);
                    }
                }
            }
            seen.Add(possiblePoint);
            toSee.Remove(possiblePoint);

            if (!seen.Contains(neighbour1) && !toSee.Contains(neighbour1))
                toSee.Add(neighbour1);
            if (!seen.Contains(neighbour2) && !toSee.Contains(neighbour2))
                toSee.Add(neighbour2);
            if (!seen.Contains(neighbour3) && !toSee.Contains(neighbour3))
                toSee.Add(neighbour3);
            if (!seen.Contains(neighbour4) && !toSee.Contains(neighbour4))
                toSee.Add(neighbour4);
        }

        Vector3? toReturn = null;
        int max = 1;
        foreach (Vector3 point in possiblePoints.Keys)
        {
            float initialHeight = additiveCrouchHeigh.y + distanceForPointEvaluation;
            int pointMax = 1;
            while (!this.CanHit(new Vector3(point.x, point.y + initialHeight, point.z), enemyPeak, enemy))
            {
                initialHeight += distanceForPointEvaluation;
                pointMax++;
            }
            if (pointMax > max)
            {
                max = pointMax;
                toReturn = point;
            }
        }

        if (toReturn.HasValue)
        {
            this.coverPointsMap[toReturn.Value].bookPoint(agent);
        }
        else
        {
            if (!this.cache.ContainsKey(enemyGridPosition))
                this.cache.Add(enemyGridPosition, new());
            this.cache[enemyGridPosition].Add(agentGridPosition);
        }

        return toReturn;
    }

    public void FreeCover(GameObject agent)
    {
        foreach (CoverPoint point in this.coverPoints)
        {
            if (point.isOccupiedBy(agent))
            {
                point.freePoint();
            }
        }
    }

    public Vector3[] GetReservedPoints(GameObject agent)
    {
        List<Vector3> reservedPoints = new();
        foreach (CoverPoint point in this.coverPoints)
        {
            if(point.isOccupiedBy(agent))
            {
                reservedPoints.Add(point.getPosition());
            }
        }
        return reservedPoints.ToArray();
    }

    private Vector3 Gridify(Vector3 position, float gridSize)
    {
        float newX = Mathf.Round(position.x / gridSize) * gridSize;
        float newY = Mathf.Round(position.y / gridSize) * gridSize;
        float newZ = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(newX, newY, newZ);
    }

    private Vector3 GetNearestPoint(Vector3 position)
    {
        return this.coverPoints.OrderBy(point => Vector3.Distance(position, point.getPosition())).First().getPosition();
    }

    private bool CanHit(Vector3 startingPosition, Vector3 destination, GameObject target)
    {
        Vector3 direction = destination - startingPosition;
        RaycastHit[] hits = Physics.RaycastAll(startingPosition, direction);
        if (Physics.Raycast(startingPosition, direction, out RaycastHit hit))
        {
            if (hit.collider.gameObject == target)
            {
                return true;
            }
            if(hit.collider.gameObject.GetComponent<CoverAgent>() != null)
            {
                float minDistance = Mathf.Infinity;
                bool targetHit = false;
                float targetDistance = Mathf.Infinity;
                foreach (RaycastHit objectHit in hits)
                {
                    if (objectHit.collider.gameObject.GetComponent<CoverAgent>() != null)
                        continue;
                    if (objectHit.collider.gameObject == target)
                    {
                        targetHit = true;
                        targetDistance = objectHit.distance;
                        continue;
                    }
                    minDistance = Mathf.Min(minDistance, objectHit.distance);
                }
                if(!targetHit)
                    return false;
                if(targetDistance < minDistance)
                    return true;
            }
        }
        return false;
    }

    // Draw gizmos
    private void OnDrawGizmos()
    {
        // Draw spawns
        Gizmos.color = Color.red;
        foreach (var spawn in this.spawns)
        {
            Gizmos.DrawCube(spawn, Vector3.one);
        }

        // Draw cover points
        foreach (var point in this.coverPoints)
        {
            point.drawGizmo();
        }
    }
}
