using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshAgentDelegates : CoverAgentDelegates
{
    public float deathAuraRadius = 1f;
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 7f;
    public float crouchSpeed = 1.5f;
    public override void Crouch()
    {
        GetComponent<NavMeshAgent>().speed = this.crouchSpeed;
        this.SetColor(Color.grey);
    }

    public override void MeleeAttack(GameObject target)
    {
        this.SetColor(Color.yellow);
    }

    public override void MoveTo(Vector3 targetPosition)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = targetPosition;
    }

    public override void RangedAttack(GameObject target)
    {
        this.SetColor(Color.magenta);
    }

    public override void Stand()
    {
        GetComponent<NavMeshAgent>().speed = this.walkingSpeed;
        this.SetColor(Color.green);
    }

    public override void Die()
    {
        GetComponent<NavMeshAgent>().speed = 0f;
        GetComponent<NavMeshAgent>().angularSpeed = 0f;
        this.SetColor(Color.black);
    }

    public override bool IsDead()
    {
        return Vector3.Distance(this.transform.position, this.enemy.transform.position) < this.deathAuraRadius;
    }

    public override float DistanceTo(GameObject target)
    {
        float distance = 0f;
        NavMeshPath path = new NavMeshPath();
        Vector3 startingPosition = this.transform.position;
        Vector3 endingPosition = target.transform.position;
        NavMesh.CalculatePath(startingPosition, endingPosition, NavMesh.AllAreas, path);
        Vector3[] points = path.corners;
        for (int i = 1; i < points.Length; i++)
        {
            distance += Vector3.Distance(points[i - 1], points[i]);
        }
        return distance;
    }

    public override void Charge()
    {
        GetComponent<NavMeshAgent>().speed = this.runningSpeed;
        this.SetColor(Color.cyan);
    }

    private void SetColor(Color c)
    {
        foreach (var material in GetComponent<Renderer>().materials.ToList())
        {
            material.SetColor("_Color", c);
        }
    }
}
