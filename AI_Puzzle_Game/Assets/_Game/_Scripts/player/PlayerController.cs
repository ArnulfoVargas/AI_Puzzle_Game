using DG.Tweening;
using UnityEngine;

public class PlayerController : BaseBehaviour {
    [SerializeField] Transform playerVisuals;
    [SerializeField] Transform rayPosition;
    [SerializeField] LayerMask borderLayer, tilesLayer;
    private PlayerInputsReader inputs;
    private float moveSpeed {get; set;}
    private float acceleration {get; set;}
    private float currentSpeed;
    private Vector3 moveDirection;
    [SerializeField] PlayerState playerState = PlayerState.IDLE;
    private Vector3 target;
    private float minDistance;

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

    public void OnTeleportEnd() {
        if (Physics.Raycast(rayPosition.position, Vector3.down, out RaycastHit f, 1f, tilesLayer)) {
            var bounds = f.collider.bounds;
            transform.position = new Vector3(bounds.min.x, 0, bounds.min.z);
        }
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
            if (Physics.Raycast(rayPosition.position, dir, out RaycastHit hit, 40f, borderLayer)) {
                if (hit.distance < 1f) return;

                if (Physics.Raycast(hit.point - dir, Vector3.down, out RaycastHit f, 1f, tilesLayer)) {
                    var bounds = f.collider.bounds;
                    target = new Vector3(bounds.min.x, 0, bounds.min.z);
                }
            }

            moveDirection = dir;
            minDistance = int.MaxValue;

            SetAnimationState();
        }
    }

    protected override void OnGameplayUpdate()
    {
        if (CurrentPlayerState == PlayerState.MOVING)
        {
            var cDistance = Vector3.Distance(transform.position, target);

            currentSpeed += Time.fixedDeltaTime * acceleration;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, moveSpeed);
            transform.Translate(moveDirection * (currentSpeed * Time.fixedDeltaTime));

            if (cDistance < minDistance) minDistance = cDistance;
            if (cDistance < .1f || cDistance > minDistance) {
                OnArriveToTarget();
            }
        }
    }
    void OnArriveToTarget(bool clipToTarget = true)
    {
        CurrentPlayerState = PlayerState.IDLE;
        moveDirection = Vector3.zero;
        // if (debugTile) {
            // if (Physics.Raycast(rayPosition.position, Vector3.down, out RaycastHit hit, 1f, tilesLayer))
            // {
            //     var b = hit.collider.bounds;
            //     var p = new Vector3(b.min.x, 0, b.min.z);
            if (clipToTarget)
                transform.position = target;
            // }
        // }
        // else {
        //     var p = transform.position;
        //     transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
        // }
    }
}