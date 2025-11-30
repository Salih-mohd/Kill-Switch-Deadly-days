using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunRigController : MonoBehaviourPun
{
    [Header("rig and weight")]
    [SerializeField] Rig handRig;
    [SerializeField] float handRigWeightIdle=0;
    [SerializeField] float handRigWightWithGun=1;

    [SerializeField] Rig aimRig;
    

    [Header("Right Hand IK")]
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform rightHandHint;

    [Header("Left Hand IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform leftHandHint;
    [SerializeField] private float leftHandIdleWeight = 0.2f;
    [SerializeField] private float leftHandAimWeight = 0.6f;

    private IGun currentGun;
    public GunAttackHandler attackHandler;


    public void ApplyIK(IGun gun)
    {
        currentGun = gun;
        

        // Right hand
        //rightHandTarget.position = gun.IKRightHandIdlePos.position;
        //rightHandTarget.rotation = gun.IKRightHandIdlePos.rotation;
        //rightHandHint.position = gun.IKRightHandHintPos.position;
        //rightHandHint.rotation = gun.IKRightHandHintPos.rotation;
        //rightHandIK.weight = 1f;

        // Left hand (idle)
        leftHandTarget.position = gun.IKLeftHandIdlePos.position;
        leftHandTarget.rotation = gun.IKLeftHandIdlePos.rotation;
        leftHandIK.weight = leftHandIdleWeight;
    }

    public void UpdateLeftHandAim(bool isAiming)
    {
  
        if (isAiming)
        {
            aimRig.weight = 1;
            leftHandTarget.position = currentGun.IKLeftHandAimPos.position;
            leftHandTarget.rotation = currentGun.IKLeftHandAimPos.rotation;
            leftHandAimWeight = currentGun.LeftHAimW;
            leftHandIK.weight = leftHandAimWeight;
        }
        else
        {
            aimRig.weight=0;
            leftHandTarget.position = currentGun.IKLeftHandIdlePos.position;
            leftHandTarget.rotation = currentGun.IKLeftHandIdlePos.rotation;
            leftHandIdleWeight = currentGun.LeftHIdleW;
            leftHandIK.weight = leftHandIdleWeight;
        }
    }

    private void Update()
    {

        if (!photonView.IsMine) return;


        if(currentGun!=null)
        {
            handRig.weight = handRigWightWithGun;
            if(attackHandler.isAttacking)
            {
                leftHandTarget.position = currentGun.IKLeftHandAimPos.position;
                leftHandTarget.rotation = currentGun.IKLeftHandAimPos.rotation;
                leftHandAimWeight = currentGun.LeftHAimW;
                leftHandIK.weight = leftHandAimWeight;
            }
            else
            {
                leftHandTarget.position = currentGun.IKLeftHandIdlePos.position;
                leftHandTarget.rotation = currentGun.IKLeftHandIdlePos.rotation;
                leftHandIdleWeight = currentGun.LeftHIdleW;
                leftHandIK.weight = leftHandIdleWeight;
            }

        }
        else
        {
            handRig.weight = handRigWeightIdle;
        }

        
        
    }

    public void ClearIK()
    {
        rightHandIK.weight = 0f;
        leftHandIK.weight = 0f;
        currentGun = null;
    }

   
}