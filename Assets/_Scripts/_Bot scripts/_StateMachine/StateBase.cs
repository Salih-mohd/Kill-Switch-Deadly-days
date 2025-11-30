using UnityEngine;

public abstract class StateBase
{
    protected BotController ai;

    public StateBase(BotController ai) { this.ai = ai; }

    public abstract void Enter();
    public abstract void Check();
    public abstract void Leave();

}
