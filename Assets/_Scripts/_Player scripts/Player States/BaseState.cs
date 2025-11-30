using System.Security.Cryptography;
using UnityEngine;

public abstract class BaseState 
{
    protected PlayerMovement playerMovement;

    public BaseState(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public abstract void Enter();
    public abstract void Check();
    public abstract void Exit();

}
