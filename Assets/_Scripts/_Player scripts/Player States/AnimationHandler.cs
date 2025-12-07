using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AnimationHandler : MonoBehaviourPun
{
    private Animator animator;
    private PhotonAnimatorView animatorView;

    [Header("ragdoll rb's")]
    [SerializeField] private List<Rigidbody> rigidbodies;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private List<Collider> hitColliders;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }


    public void SetSpeed(float speed, float dampTime)
    {
        animator.SetFloat("Speed", speed, dampTime, Time.deltaTime);
    }

    public void SetGunState(bool hasGun)
    {
        animator.SetBool("TookGun", hasGun);
    }

    public void TriggerJump(bool withGun)
    {
        animator.SetTrigger(withGun ? "JumpWithGun" : "Jump");

    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attaack");
    }

    public void SetAttackState(bool isAttacking)
    {
        animator.SetBool("Attack", isAttacking);
    }

    public void SetDieState()
    {
        animator.SetTrigger("Died");
    }





    [PunRPC]
    private void Died()
    {

         
        animator.SetTrigger("Died");
        
    }

}