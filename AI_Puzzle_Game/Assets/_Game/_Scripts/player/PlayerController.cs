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
    [SerializeField] PlayerState playerState = PlayerState.ANIMATION;
    [SerializeField] private Vector3 target;
    [SerializeField] private Transform particlePosition;
    private float minDistance;
    private bool shouldPlayHitParticle;
    private Vector3 hitParticlePoint;
    IPlayerMovementSoundEmmiter soundEmmiter;

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
        ParticlesManager.Instance.SpawnParticle(ParticleType.RUN, particlePosition.position, Quaternion.LookRotation(-moveDirection, Vector3.up));
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

        PositionatePlayer();
    }

    void OnDisable()
    {
        inputs.OnMove -= MoveTowards;
    }

    private void MoveTowards(Vector3 dir)
    {
        if (CurrentPlayerState != PlayerState.IDLE)
            return;
            
        if (Physics.Raycast(rayPosition.position, dir, out RaycastHit hit, 40f, borderLayer))
        {
            EvaluateRaycastHitForSound(hit);

            if (hit.distance < 1f) {
                PlayEmmiterSound();
            }

            if (Physics.Raycast(hit.point - (dir * .5f), Vector3.down, out RaycastHit f, 1f, tilesLayer))
            {
                var bounds = f.collider.bounds;
                var newTarget = new Vector3(bounds.min.x, 0, bounds.min.z);
                if (target == newTarget) return;

                target = newTarget;

                if (hit.collider.TryGetComponent(out MeshRenderer mr))
                {
                    shouldPlayHitParticle = true;
                    hitParticlePoint = hit.point;
                }
                else
                {
                    shouldPlayHitParticle = false;
                }
            }
        }

        moveDirection = dir;
        minDistance = int.MaxValue;

        AudioManager.GetInstance().SetAudioWithZeroPosition(Audio_Type.SLIDE);

        // SetAnimationState();
    }

    private void EvaluateRaycastHitForSound(RaycastHit hit) {
        if (hit.collider.TryGetComponent(out IPlayerMovementSoundEmmiter emmiter)) {
            soundEmmiter = emmiter;
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
        PlayEmmiterSound();
        if (shouldPlayHitParticle)
            ParticlesManager.Instance.SpawnParticle(ParticleType.COLLISION, hitParticlePoint);

        // }
        // }
        // else {
        //     var p = transform.position;
        //     transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
        // }
    }

    private void PlayEmmiterSound()
    {
        soundEmmiter?.PlaySound();
        soundEmmiter = null;
    }

    private void PositionatePlayer() {
        if (Physics.Raycast(rayPosition.position, Vector3.down, out RaycastHit f, 1f, tilesLayer))
        {
                var bounds = f.collider.bounds;
                var newTarget = new Vector3(bounds.min.x, 0, bounds.min.z);
                transform.position = newTarget;
        }
    }
}