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
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = this.crouchSpeed;
        agent.updateRotation = true;
        this.SetColor(Color.grey);
    }

    public override void MeleeAttack(GameObject target)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = 0f;
        agent.updateRotation = false;
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
        this.SetColor(Color.yellow);
    }

    public override void MoveTo(Vector3 targetPosition)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.destination = targetPosition;
    }

    public override void RangedAttack(GameObject target)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = 0f;
        agent.updateRotation = false;
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
        this.SetColor(Color.magenta);
    }

    public override void Stand()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = this.walkingSpeed;
        agent.updateRotation = true;
        this.SetColor(Color.green);
    }

    public override void Die()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = 0f;
        agent.angularSpeed = 0f;
        this.SetColor(Color.black);
    }

    public override bool IsDead()
    {
        return Vector3.Distance(this.transform.position, this.enemy.transform.position) < this.deathAuraRadius;
    }

    public override float DistanceBetween(Vector3 startPosition, Vector3 endPosition)
    {
        float distance = 0f;
        NavMeshPath path = new();
        NavMesh.CalculatePath(startPosition, endPosition, NavMesh.AllAreas, path);
        Vector3[] points = path.corners;
        for (int i = 1; i < points.Length; i++)
        {
            distance += Vector3.Distance(points[i - 1], points[i]);
        }
        return distance;
    }

    public override void Charge()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = this.runningSpeed;
        agent.updateRotation = true;
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
