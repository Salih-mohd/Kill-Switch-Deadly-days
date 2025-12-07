using UnityEngine;

public class DieState : StateBase
{
    public DieState(BotController ai) : base(ai)
    {
    }

    public override void Check()
    {
         
    }

    public override void Enter()
    {
        ai.Animator.SetTrigger("Dead");
        ai.DestroyAfterDeath();
    }

    public override void Leave()
    {
         
    }
}
