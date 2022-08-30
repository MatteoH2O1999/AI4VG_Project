using System;

public class BeginAttackTransition : FSMTransition
{
    public override bool FireTransition(CoverAgent agent)
    {
        Random rnd = new Random();
        return rnd.NextDouble() >= 0.9;
    }

    public override void TransitionActions(CoverAgent agent)
    {
    }
}