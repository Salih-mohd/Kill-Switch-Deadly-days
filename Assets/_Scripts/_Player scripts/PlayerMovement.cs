using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviourPunCallbacks
{
    // public variables
    public float playerSpeedWhenFiring = 3f;
    public float playerIdleSpeed = 5f;
    public float playerHoldingGunSpeed = 4.7f;
    public float playerSpeed;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public GameObject camTrackingTarget;
    
    public bool groundedPlayer;
    public float moveSpeed;
    public float dampTime = .1f;



    // private variables
    private CinemachineCamera freeLookCamTransform;

    private PlayerInputHandler inputHandler;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private AnimationHandler animationHandler;
    private GunPickupHandler gunPickupHandler;
    private GunAttackHandler attackHandler;

    private PhotonView pv;

    

    bool test;

    private void Awake()
    {

        pv = GetComponent<PhotonView>();
        controller =GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        gunPickupHandler = GetComponent<GunPickupHandler>();
        gunPickupHandler.speedWithoughtGun = playerIdleSpeed;


        attackHandler = GetComponent<GunAttackHandler>();

        attackHandler.attackingSpeed=playerSpeedWhenFiring;
        attackHandler.gunHoldingSpeed=playerHoldingGunSpeed;

        playerSpeed = playerIdleSpeed;

    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Find the single Cinemachine Virtual Camera in the scene
            freeLookCamTransform = FindFirstObjectByType<CinemachineCamera>();

            // Assign this player as the follow/look target
            freeLookCamTransform.Follow=camTrackingTarget.transform;
        }

    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (!IsLocalPlayer()) return;
        gunPickupHandler.gunPicked += ChangingPlayerSpeed;
        gunPickupHandler.gunDropped += ChangingPlayerSpeed;
        //attackHandler.AttackSpeed += ChangingPlayerSpeed;
        attackHandler.AttackEvent += ChangingPlayerSpeed;
        attackHandler.AttackDoneEvent += ChangingPlayerSpeed;
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if (!IsLocalPlayer()) return;
        gunPickupHandler.gunDropped -= ChangingPlayerSpeed;
        gunPickupHandler.gunPicked -= ChangingPlayerSpeed;
        //attackHandler.AttackSpeed -= ChangingPlayerSpeed;
        attackHandler.AttackEvent -= ChangingPlayerSpeed;
        attackHandler.AttackDoneEvent -= ChangingPlayerSpeed;
    }

    void Update()
    {

        if (!IsLocalPlayer()) return;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Read input
        Vector2 input = inputHandler.MoveInput;

        // directon set up
        Vector3 camForward = freeLookCamTransform.gameObject.transform.forward;
        Vector3 camRight = freeLookCamTransform.gameObject.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();


        // when attacking
        Vector3 cameraDirection = new Vector3(camForward.x, 0f, camForward.z).normalized;



        Vector3 move = camForward * input.y + camRight * input.x;
        move = Vector3.ClampMagnitude(move, 1f);

        //var camDir = camForward + camRight;




        // animation
        moveSpeed = move.magnitude;

        if (groundedPlayer)
        {
            //animator.SetFloat("Speed",moveSpeed,dampTime,Time.deltaTime);
            animationHandler.SetSpeed(moveSpeed, dampTime);

        }


        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        if (gunPickupHandler.HasGunEquipped && attackHandler.isAttacking)
        {          
            transform.forward = cameraDirection; 
        }




        // Jump
        if (inputHandler.JumpPressedThisFrame && groundedPlayer)
        {
            

            
            if (gunPickupHandler.HasGunEquipped)
            {
                animationHandler.TriggerJump(true);
                 
            }
            else
            {
                animationHandler.TriggerJump(false);
            }


            

            //animator.SetTrigger("Jump");    
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement

        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }

    private void ChangingPlayerSpeed(float speed)
    {
         
        playerSpeed = speed;
    }

    private bool IsLocalPlayer()
    {
        return pv != null && pv.IsMine;
    }



}
