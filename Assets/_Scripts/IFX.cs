using UnityEngine;

public interface IFX
{

    int FXEvent { get;}
    void PlayFx(Vector3 position,Quaternion rotation);
}
