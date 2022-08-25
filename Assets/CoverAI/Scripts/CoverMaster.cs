using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoverDelegates))]
public class CoverMaster : MonoBehaviour
{

    public float distanceBetweenPoints = 1.0f;
    public List<Vector3> spawns;

    private readonly HashSet<CoverPoint> coverPoints = new();

    // Start is called before the first frame update
    void Start()
    {
        this.UpdatePoints();
    }

    // Generate cover points
    void UpdatePoints()
    {
        HashSet<Vector3> coverPositions = new();

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
            if (!coverPositions.Contains(borderPoint))
            {
                coverPositions.Add(borderPoint);
                this.coverPoints.Add(new CoverPoint(borderPoint));
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

    public CoverPoint GetCover(GameObject agent, GameObject enemy, float gunHeight)
    {
        // TODO
        return null;
    }

    public void FreeCover(GameObject agent)
    {
        this.coverPoints.RemoveWhere(point => point.isOccupiedBy(agent));
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
