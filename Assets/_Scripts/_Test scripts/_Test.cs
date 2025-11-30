using UnityEngine;
using UnityEngine.Animations.Rigging;
public class _Test : MonoBehaviour
{
    public bool atGun;
    //public Transform rightHandGunPos;
    public GameObject gun;
    private GameObject currentGun;

    public Animator animator;


    [Header("right hand target")]
    public TwoBoneIKConstraint RightHandtwoBoneIk;
    public Transform rightHandTarget;
    public Transform rightHandHint;

    [Header("left hand target")]
    public TwoBoneIKConstraint LeftHandTwoBoneIk;
    public Transform leftHandTarget;
    public Transform leftHandHint;
    //public Transform leftHandTargetAim;
    public float weightOfLeftIK=.2f;
    public float weightOfLeftIkAim = .6f;


    [Header("values on guns")]
    public Transform IKRightHandPos;
    public Transform IKRightHandHintPos;

    [Header("values on guns (left hand)")]
    public Transform IKLeftHandPos;
    public Transform IKLeftHandPosAim;

    public Transform IKleftHandHintIdle;
    public Transform IKleftHandHintAim;


    [Header("Aiming settings")]
    public Rig aimRig;
    public float aimRigWeight;
    public Vector3 AimPos;


    
  

    public Transform EquipPos;
    public bool isGunPicked;
    public bool isAttacking;

    public PlayerInputHandler inutHandler;
    public static _Test instance;

    private void Awake()
    {
        instance = this;
    }



    private void OnTriggerEnter(Collider other)
    {
        
        atGun = true;
        gun=other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        atGun = false;
        gun = null;
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.U))
        {
            aimRig.weight = 1;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            aimRig.weight = 0;
        }


        //    if (inutHandler.GrabbedThisFrame)
        //    {
        //        Debug.Log("pressed the g key");
        //        if (atGun)
        //        {
        //            currentGun = gun;
        //            Debug.Log("inside at gun check");
        //            animator.SetBool("TookGun", true);

        //            isGunPicked = true;

        //        }
        //    }

        //    if (isGunPicked)
        //    {

        //        currentGun.transform.SetParent(EquipPos.transform);
        //        currentGun.transform.localPosition = Vector3.zero;
        //        currentGun.transform.localRotation = Quaternion.identity;


        //        //RightHandtwoBoneIk.weight = 1;

        //        //rightHandTarget.position = IKRightHandPos.position;
        //        //rightHandTarget.rotation = IKRightHandPos.rotation;

        //        if (!isAttacking)
        //        {
        //            LeftHandTwoBoneIk.weight = weightOfLeftIK;




        //            // handling left Hand

        //            leftHandTarget.position = IKLeftHandPos.position;
        //            leftHandTarget.rotation = IKLeftHandPos.rotation;

        //            //leftHandHint.position=IKleftHandHintIdle.position;
        //            //rightHandHint.position=IKRightHandHintPos.position;
        //            //rightHandHint.rotation=IKRightHandHintPos.rotation;
        //        }

        //        if (isAttacking)
        //        {
        //            LeftHandTwoBoneIk.weight = weightOfLeftIkAim;
        //            //LeftHandTwoBoneIk.weight = weightOfLeftIK;
        //            leftHandTarget.position = IKLeftHandPosAim.position;
        //            leftHandTarget.rotation = IKLeftHandPosAim.rotation;
        //            //leftHandHint = IKleftHandHintAim;
        //        }


        //        if(inutHandler.isAttacking)
        //        {
        //            animator.SetTrigger("Attaack");
        //            isAttacking = true;
        //            animator.SetBool("Attack", true);
        //        }
        //        else if(inutHandler.isNotAttacking)
        //        {

        //            animator.SetBool("Attack", false);
        //            isAttacking = false;


        //        }

        //    }



        //    if(inutHandler.DetachedThisFrame)
        //    {
        //        if (atGun)
        //        {
        //            if (currentGun != null)
        //            {
        //                currentGun.transform.SetParent(null);
        //                isGunPicked = false;
        //                currentGun = null;
        //                animator.SetBool("TookGun", false);
        //            }

        //        }
        //    }
        //}
    }
}
