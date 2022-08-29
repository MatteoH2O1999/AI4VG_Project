using UnityEngine;

public class DeadState : FSMState
{
    private float deathTime;
    public override void OnEntry(CoverAgent agent)
    {
        agent.GetDelegates().Die();
        this.deathTime = Time.time;
    }

    public override void OnExit(CoverAgent agent)
    {
    }

    public override void OnInterrupt(CoverAgent agent)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStay(CoverAgent agent)
    {
        if(Time.time - this.deathTime > 2.0f)
            Object.Destroy(agent.gameObject);
    }
}