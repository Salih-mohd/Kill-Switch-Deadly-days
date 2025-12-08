using UnityEngine;

public interface IGun
{
    Transform IKRightHandIdlePos { get; }
    Transform IKRightHandHintPos { get; }
    Transform IKLeftHandIdlePos { get; }
    Transform IKLeftHandAimPos { get; }

    Transform MuzzleFlash {  get; }


    float moveSpeed {  get; }
    float LeftHIdleW {  get; }
    float LeftHAimW { get; }


    GunData GunData {  get; }
    int CurrentAmmo { get; set; }

    int RecerveAmmo { get; set; }


    void OnEquip(Transform playerRoot);
    void OnUnequip();
    

}
