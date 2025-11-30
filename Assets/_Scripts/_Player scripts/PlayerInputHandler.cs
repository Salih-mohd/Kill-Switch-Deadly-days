using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviourPunCallbacks
{
    [Header("Input actions")]
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    public InputAction attackAction;
    private InputAction grabAction;
    private InputAction detachAction;
    private InputAction reloadAction;
    private InputAction pauseAction;


    private PhotonView pv;
    private LocalUIManager localUIManager;

    private bool isPaused;

    public Vector2 MoveInput { get; private set; }
    public bool JumpPressedThisFrame { get; private set; }
    public Vector2 LookDelta { get; private set; }

    public bool isAttacking;
    public bool isNotAttacking;

    public bool GrabbedThisFrame {  get; private set; }
    public bool DetachedThisFrame { get; private set; }

    public bool fire;

    public event Action ReloadEvent;

    public float sensX;
    public float sensY;

    // test
    bool test;

    private void Awake()
    {
        pv=GetComponent<PhotonView>();
        //localUIManager=GetComponentInChildren<LocalUIManager>();
        moveAction = inputActions.FindAction("Move");
        jumpAction = inputActions.FindAction("Jump");
        lookAction = inputActions.FindAction("Look");
        attackAction = inputActions.FindAction("Attack");
        grabAction = inputActions.FindAction("Grab");
        detachAction = inputActions.FindAction("Detach");
        reloadAction = inputActions.FindAction("Reload");
        pauseAction = inputActions.FindAction("Pause");




        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCanceled;

        lookAction.performed += OnLookPerformed;
        lookAction.canceled += OnLookCanceled;

        jumpAction.performed += OnJumpPerformed;

        //attackAction.performed += OnAttackStarted;
        //attackAction.canceled += OnAttackEnded;

        grabAction.performed += OnGrab;
        
        //detachAction.performed += OnDetach;

        reloadAction.performed += OnReload;

        pauseAction.performed += OnPause;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        inputActions.FindActionMap("Player").Enable();
        //localUIManager.OnDiedEvent += DisablingPlayerMap;
        GlobalUIManager.instance.OnMatchEnded += DisablingPlayerMap;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        inputActions.FindActionMap("Player").Disable();
        //localUIManager.OnDiedEvent -= DisablingPlayerMap;
        GlobalUIManager.instance.OnMatchEnded -= DisablingPlayerMap;
    }


    private void Update()
    {

        if (!IsLocalPlayer()) return;

        // attack

        isAttacking = attackAction.WasPerformedThisFrame();
        isNotAttacking = attackAction.WasReleasedThisFrame();
        DetachedThisFrame = detachAction.WasPerformedThisFrame();
        float attackValue = attackAction.ReadValue<float>();
        fire = attackValue > 0.5f;






        // input testing;

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    test = !test;

        //    if (test)
        //    {
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //    }
        //    else
        //    {
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    }
            
        //}


        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GunAttackHandler.instance.Reload();
        //}


        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    Debug.Log("I is pressed");
        //    GlobalUIManager.instance.ShowScoreBoard();
        //}

        //if(Input.GetKeyDown(KeyCode.O)) GlobalUIManager.instance.HideScoreBoard();
        

    }


    private void LateUpdate()
    {

        if (!IsLocalPlayer()) return;

        JumpPressedThisFrame =false;
        GrabbedThisFrame = false;
         
        
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        MoveInput=ctx.ReadValue<Vector2>(); 
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        MoveInput = Vector2.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        //Vector2 rawDelta = ctx.ReadValue<Vector2>();
        //LookDelta = new Vector2(rawDelta.x * sensX, rawDelta.y * sensY);

        LookDelta = ctx.ReadValue<Vector2>();
    }
    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        LookDelta = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        //Debug.Log("called jump performed");
        JumpPressedThisFrame = true;
    }

    private void OnGrab(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        GrabbedThisFrame =true;        
    }

    private void OnDetach(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        DetachedThisFrame =true;
    }


    private void OnReload(InputAction.CallbackContext ctx)
    {
        if (!IsLocalPlayer()) return;
        ReloadEvent?.Invoke();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {

        if (!IsLocalPlayer()) return;
        if (!isPaused)
        {
            isPaused = true;
            GlobalUIManager.instance.ShowScoreBoard();
            
        }
        else
        {
            isPaused = false;
            GlobalUIManager.instance.HideScoreBoard();
        } 
            

 
    }



    public void DisablingPlayerMap()
    {
        inputActions.FindActionMap("Player").Disable();
        
    }


    private bool IsLocalPlayer()
    {
        return pv != null && pv.IsMine;
    }

}