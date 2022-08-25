using System.Collections.Generic;

public abstract class FSMState
{
    private readonly Dictionary<FSMTransition, FSMState> links = new();
    public abstract void OnEntry(CoverAgent agent);
    public abstract void OnExit(CoverAgent agent);
    public abstract void OnStay(CoverAgent agent);
    public abstract void OnInterrupt(CoverAgent agent);
    public void AddLink(FSMTransition transition, FSMState state)
    {
        this.links.Add(transition, state);
    }
    public (FSMState, FSMTransition) NextState(CoverAgent agent)
    {
        foreach (var link in this.links)
        {
            if (link.Key.FireTransition(agent))
                return (link.Value, link.Key);
        }
        return (null, null);
    }
}