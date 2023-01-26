using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Animator vfxAnimator;
    private Rigidbody2D rb;
    private PlayerMove pm;

    [Header("changable")]

    [Header("For Debug")]
    public bool isAttacking;
    public bool isSheilding; 
    public bool isParrying;

    [SerializeField] float AttackKind  = 0f;

    [Space]
    [SerializeField] float LastXInput = 0f;
    [SerializeField] float LastYInput = -1f;
    [SerializeField] float XInput = 0f;
    [SerializeField] float YInput = 0f;
    

    private Player player;

    private void Awake() {
        animator = GetComponent<Animator>();
        vfxAnimator = GetComponentInChildren<VFX>().gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMove>();

        player = ReInput.players.GetPlayer(0);

        player.AddInputEventDelegate(UPPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Move Up");
        player.AddInputEventDelegate(DownPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Move Down");
        player.AddInputEventDelegate(RightPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed,"Move Right");
        player.AddInputEventDelegate(LeftPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed,"Move Left");

        player.AddInputEventDelegate(UPJustPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Move Up");
        player.AddInputEventDelegate(DownJustPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Move Down");
        player.AddInputEventDelegate(RightJustPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed,"Move Right");
        player.AddInputEventDelegate(LeftJustPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed,"Move Left");

        player.AddInputEventDelegate(UPJustReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Move Up");
        player.AddInputEventDelegate(DownJustReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Move Down");
        player.AddInputEventDelegate(RightJustReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased,"Move Right");
        player.AddInputEventDelegate(LeftJustReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased,"Move Left");
    }

    void Update()
    {
        if(GameManager.instance.isPlayerPaused) return;
        
        SetWalkAnimation();
        SetAttackAnimation();
        SetShieldAnimation();
        SetParryAnimation();
    }

    private void SetPlayerMove(bool flag)
    {
        pm.canMove = flag;
    }

    private void SetWalkAnimation()
    {
        animator.SetFloat("XInput", XInput);
        animator.SetFloat("YInput", YInput);
        animator.SetFloat("LastXInput", LastXInput);
        animator.SetFloat("LastYInput", LastYInput);
    }

    private void SetAttackAnimation()
    {
        if(player.GetButtonDown("Attack"))
        {
            if(isAttacking) return;
            if(isSheilding) return;
            if(isParrying) return;

            animator.SetFloat("AttackKind", AttackKind);
            animator.SetTrigger("Attack");
            isAttacking = true;
            SetPlayerMove(false);
            
        }
    }

    private void SetShieldAnimation()
    {
        if(isParrying) return;

        if(player.GetButton("Shield"))
        {
            SetPlayerMove(false);
        }

        if(player.GetButtonUp("Shield"))
        {
            SetPlayerMove(true);
        }


        isSheilding = player.GetButton("Shield");
        animator.SetBool("Shield", isSheilding);
          
    }

    private void SetParryAnimation()
    {
        if(player.GetButton("Shield") && player.GetButtonDown("Interactive"))
        {
            if(isParrying) return;
            animator.SetTrigger("Parry");
            isParrying = true;
        }
    }

    //input Aniamtion Event

    // keep presseing
    void UPPressed(InputActionEventData data)
    {
        if(isAttacking || isSheilding || isParrying ) return;
        LastXInput = 0f;
        LastYInput = 1f;
    }

    void DownPressed(InputActionEventData data)
    {
        if(isAttacking || isSheilding || isParrying ) return;
        LastXInput = 0f;
        LastYInput = -1f;
    }

    void RightPressed(InputActionEventData data)
    {
        if(isAttacking || isSheilding || isParrying ) return;
        LastXInput = 1f;
        LastYInput = 0f;
    }

    void LeftPressed(InputActionEventData data)
    {
        if(isAttacking || isSheilding || isParrying ) return;
        LastXInput = -1f;
        LastYInput = 0f;
    }
    // keep presseing


    // just the time press the button
    void UPJustPressed(InputActionEventData data)
    {
        XInput = 0f;
        YInput = 1f;
    }

    void DownJustPressed(InputActionEventData data)
    {
        XInput = 0f;
        YInput = -1f;
    }

    void RightJustPressed(InputActionEventData data)
    {
        XInput = 1f;
        YInput = 0f;
    }

    void LeftJustPressed(InputActionEventData data)
    {
        XInput = -1f;
        YInput = 0f;
    }
    // just the time press the button

    // just the time release the button
    void UPJustReleased(InputActionEventData data)
    {
        if(player.GetButton("Move Right"))
        {
            XInput = 1f;
            YInput = 0f;
        }else if(player.GetButton("Move Left"))
        {
            XInput = -1f;
            YInput = 0f;
        }else if(player.GetButton("Move Down"))
        {
            XInput = 0f;
            YInput = -1f;
        }else
        {
            XInput = 0f;
            YInput = 0f;
        }
        
    }

    void DownJustReleased(InputActionEventData data)
    {        
        if(player.GetButton("Move Right"))
        {
            XInput = 1f;
            YInput = 0f;
        }else if(player.GetButton("Move Left"))
        {
            XInput = -1f;
            YInput = 0f;
        }else if(player.GetButton("Move Up"))
        {
            XInput = 0f;
            YInput = 1f;
        }else
        {
            XInput = 0f;
            YInput = 0f;
        }
    }

    void RightJustReleased(InputActionEventData data)
    {        
        if(player.GetButton("Move Up"))
        {
            XInput = 0f;
            YInput = 1f;
        }else if(player.GetButton("Move Down"))
        {
            XInput = 0f;
            YInput = -1f;
        }else if(player.GetButton("Move Left"))
        {
            XInput = -1f;
            YInput = 0f;
        }else
        {
            XInput = 0f;
            YInput = 0f;
        }
    }

    void LeftJustReleased(InputActionEventData data)
    {        
        if(player.GetButton("Move Up"))
        {
            XInput = 0f;
            YInput = 1f;
        }else if(player.GetButton("Move Down"))
        {
            XInput = 0f;
            YInput = -1f;
        }else if(player.GetButton("Move Right"))
        {
            XInput = 1f;
            YInput = 0f;
        }else
        {
            XInput = 0f;
            YInput = 0f;
        }
    }
    // just the time release the button

    //input Aniamtion Event



    //Animation event
    public void SlashStart()
    {
        
    }

    public void SlashEnd()
    {
        isAttacking = false;
        SetPlayerMove(true);
    }

    public void StabStart()
    {
    }

    public void StabEnd() 
    {
        isAttacking = false;
        SetPlayerMove(true);
    }
    public void ParryStart()
    {
        
    }

    public void ParryEnd()
    {
        isParrying = false;
        SetPlayerMove(true);
    }

    //Animation event

    private void OnDestroy() {
    }
}
