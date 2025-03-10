using DG.Tweening;
using UnityEngine;

public class PlayerController : BaseBehaviour {
    [SerializeField] Transform playerVisuals;
    [SerializeField] Transform rayPosition;
    private PlayerInputsReader inputs;
    private float moveSpeed {get; set;}
    private Vector3 moveDirection;
    PlayerState playerState = PlayerState.IDLE;
    public PlayerState CurrentPlayerState {
        get => playerState; 
        set {
            playerState = value;
            GameManager.GetInstance().CurrentPlayerState = playerState;
        }
    }

    public void SetAnimationState()
    {
        this.CurrentPlayerState = PlayerState.ANIMATION;
    }

    public void SetTravelingState()
    {
        this.CurrentPlayerState = PlayerState.TRAVELING;
    }

    public void SetIdleState()
    {
        this.CurrentPlayerState = PlayerState.IDLE;
    }
    
    protected override void OnStart()
    {
        var configs = Resources.Load<PlayerConfigs>("PlayerConfigs");
        inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        moveSpeed = configs.MoveSpeed;

        inputs.OnMove += MoveTowards;
    }

    void OnDisable()
    {
        inputs.OnMove -= MoveTowards;
    }

    private void MoveTowards(Vector3 dir) {
        if (CurrentPlayerState == PlayerState.IDLE)
        {
            if (Physics.Raycast(rayPosition.position, dir, .55f)) return;

            moveDirection = dir;

            // if (playerVisuals.forward != dir) {
            //     CurrentPlayerState = PlayerState.ROTATING;
            //     playerVisuals.DOLookAt(playerVisuals.position + moveDirection, 0.5f).OnComplete(() => {
            //         CurrentPlayerState = PlayerState.MOVING;
            //     });
            // }
            // else CurrentPlayerState = PlayerState.MOVING;

            CurrentPlayerState = PlayerState.MOVING;
        }
    }

    protected override void OnGameplayUpdate()
    {
        if (CurrentPlayerState == PlayerState.MOVING)
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }
    void OnTriggerEnter(Collider other)
    {
        CurrentPlayerState = PlayerState.IDLE;
        moveDirection = Vector3.zero;
        var p = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
    }
}