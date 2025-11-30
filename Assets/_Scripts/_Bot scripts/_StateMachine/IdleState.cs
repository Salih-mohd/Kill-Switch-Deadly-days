using NUnit.Framework.Interfaces;
using UnityEngine;

public class IdleState : StateBase
{
    public IdleState(BotController ai) : base(ai) { }

    public override void Enter()
    {
        
    }

    public override void Check()
    {
        
        ai.ChangeState(new FollowState(ai));
    }

    public override void Leave()
    {
         
    }
}