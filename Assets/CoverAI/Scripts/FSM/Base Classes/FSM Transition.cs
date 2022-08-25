public abstract class FSMTransition
{
    public abstract bool FireTransition(CoverAgent agent);
    public abstract void TransitionActions(CoverAgent agent);
}