using UnityEngine;
using UnityEngine.UI;

public class SwipeController : BaseBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float inactiveTimeForAnimation = 15f;
    [SerializeField] private SwipeStates startSwipeAnimation = SwipeStates.NONE;
    [SerializeField] private Image img;
    [SerializeField] private GameObject text;
    private PlayerInputsReader inputs;
    readonly int upRightAnim = Animator.StringToHash("UR");
    readonly int upLeftAnim = Animator.StringToHash("UL");
    readonly int downRightAnim = Animator.StringToHash("DR");
    readonly int downLeftAnim = Animator.StringToHash("DL");
    readonly int idleAnim = Animator.StringToHash("Idle");
    private SwipeStates swipeStates = SwipeStates.NONE;
    [SerializeField] private float timeInactive;
    [SerializeField] private bool triggeredAnimation;

    public static SwipeController Instance {
        get;
        private set;
    }

    protected override void OnStart()
    {
        Instance = this;

        if (startSwipeAnimation != SwipeStates.NONE) {
            TriggerAnimation(startSwipeAnimation);
            text.SetActive(true);
        }
    }

    protected override void OnUpdateState(GameState state)
    {
        img.enabled = state == GameState.GAMEPLAY;
        SetTextVisibility();
    }

    protected override void OnEnableAction()
    {
        inputs ??= Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        inputs.OnMove += OnMove;  
    }

    protected override void OnDisableAction()
    {
        inputs ??= Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        inputs.OnMove -= OnMove;  
    }

    private void OnMove(Vector3 dir) {
        TriggerAnimation(SwipeStates.NONE);
        triggeredAnimation = false;
        timeInactive = 0;
    }

    public void SetSwipeState(SwipeStates state) {
        this.swipeStates = state;
    }

    protected override void OnGameplayUpdate()
    {
        if (triggeredAnimation) return;

        timeInactive += Time.fixedDeltaTime;

        if (timeInactive >= inactiveTimeForAnimation) {
            TriggerAnimation(swipeStates);
        }
    }

    private void SetTextVisibility() {
        text.SetActive(triggeredAnimation && currentState == GameState.GAMEPLAY);
    }

    private void TriggerAnimation(SwipeStates swipeState) {
        animator.SetBool(upRightAnim, swipeState == SwipeStates.UP_RIGHT);
        animator.SetBool(upLeftAnim, swipeState == SwipeStates.UP_LEFT);
        animator.SetBool(downRightAnim, swipeState == SwipeStates.DOWN_RIGHT);
        animator.SetBool(downLeftAnim, swipeState == SwipeStates.DOWN_LEFT);
        animator.SetBool(idleAnim, swipeState == SwipeStates.NONE);

        triggeredAnimation = swipeState != SwipeStates.NONE;
        SetTextVisibility();
    }
}
