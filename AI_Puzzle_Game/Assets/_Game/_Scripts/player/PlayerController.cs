using DG.Tweening;
using UnityEngine;

public class PlayerController : BaseBehaviour {
    [SerializeField] Transform playerVisuals;
    [SerializeField] Transform rayPosition;
    private PlayerInputsReader inputs;
    private float moveSpeed {get; set;}
    private float acceleration {get; set;}
    private float currentSpeed;
    private Vector3 moveDirection;
    PlayerState playerState = PlayerState.IDLE;
    public PlayerState CurrentPlayerState {
        get => playerState; 
        set {
            playerState = value;
            currentSpeed = 0;
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

    public void SetMovingState()
    {
        this.CurrentPlayerState = PlayerState.MOVING;
    }
    
    protected override void OnStart()
    {
        var configs = Resources.Load<PlayerConfigs>("PlayerConfigs");
        inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        moveSpeed = configs.MoveSpeed;
        acceleration  = configs.Acceleration;

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

            SetAnimationState();
        }
    }

    protected override void OnGameplayUpdate()
    {
        if (CurrentPlayerState == PlayerState.MOVING)
        {
            currentSpeed += Time.deltaTime * acceleration;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, moveSpeed);
            transform.Translate(moveDirection * (currentSpeed * Time.deltaTime));
        }
    }
    void OnTriggerEnter(Collider other)
    {
        CurrentPlayerState = PlayerState.IDLE;
        moveDirection = Vector3.zero;
        var p = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
    }
}