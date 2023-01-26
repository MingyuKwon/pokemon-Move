
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PokemonPlayerMove : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;

    [Header("changeable stat")]
    public float moveSpeed = 1f;
    public bool canMove = true;


    [Header("Debug stat")]
    [SerializeField] private int PlayerState = 0; // 0 : idle, 1 : horizontal, 2 : Vertical
    [SerializeField] private float SpeedScholar;
    [SerializeField] private bool HorizontalInput;
    [SerializeField] private bool VerticalInput;

    public float HorizontalMoveSpeed;
    public float VerticalMoveSpeed;

    private void Awake() {
        player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody2D>();

        player.AddInputEventDelegate(OnMoveVerticalPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Move Up");
        player.AddInputEventDelegate(OnMoveVerticalPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Move Down");
        player.AddInputEventDelegate(OnMoveVerticalReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Move Up");
        player.AddInputEventDelegate(OnMoveVerticalReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Move Down");

        player.AddInputEventDelegate(OnMoveHorizontalPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed,"Move Right");
        player.AddInputEventDelegate(OnMoveHorizontalPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed,"Move Left");
        player.AddInputEventDelegate(OnMoveHorizontalReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased,"Move Right");
        player.AddInputEventDelegate(OnMoveHorizontalReleased, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased,"Move Left");

    }

    void Update()
    {
        if(canMove && !(GameManager.instance.isPlayerPaused))
        {
            if(player.GetButton("Move Up"))
            {
                VerticalMoveSpeed =  moveSpeed * Time.fixedDeltaTime;
            }
            if(player.GetButton("Move Down"))
            {
                VerticalMoveSpeed =  -moveSpeed * Time.fixedDeltaTime;
            }

            if(player.GetButton("Move Right"))
            {
                HorizontalMoveSpeed =  moveSpeed * Time.fixedDeltaTime;
            }
            if(player.GetButton("Move Left"))
            {
                HorizontalMoveSpeed =  -moveSpeed * Time.fixedDeltaTime;
            }

            VerticalInput = player.GetButton("Move Up") || player.GetButton("Move Down");
            HorizontalInput = player.GetButton("Move Right") || player.GetButton("Move Left");
        
            if(!VerticalInput) VerticalMoveSpeed = 0f;
            if(!HorizontalInput) HorizontalMoveSpeed = 0f;

            LockMoving();
        
            transform.position = new Vector2( transform.position.x + HorizontalMoveSpeed , transform.position.y + VerticalMoveSpeed);
        }else
        {

        }
        
        
    }

    void LockMoving()
    {
        if(PlayerState == 1)
        {
            VerticalMoveSpeed = 0f;
        }
        else if(PlayerState == 2)
        {
            HorizontalMoveSpeed = 0f;
        }
    }


    void OnMoveVerticalPressed(InputActionEventData data)
    {
        PlayerState = 2;
    }
    void OnMoveVerticalReleased(InputActionEventData data)
    {
        if(HorizontalInput)
        {
            PlayerState = 1;
        }else
        {   
            PlayerState = 0;
        }
    }

    void OnMoveHorizontalPressed(InputActionEventData data)
    {
        PlayerState = 1;
    }
    void OnMoveHorizontalReleased(InputActionEventData data)
    {
        if(VerticalInput)
        {
            PlayerState = 2;
        }else
        {
            PlayerState = 0;
        }
    }

    private void OnDestroy() {
        player.RemoveInputEventDelegate(OnMoveVerticalPressed);
        player.RemoveInputEventDelegate(OnMoveVerticalReleased);
        player.RemoveInputEventDelegate(OnMoveHorizontalPressed);
        player.RemoveInputEventDelegate(OnMoveHorizontalReleased);
    }

}
