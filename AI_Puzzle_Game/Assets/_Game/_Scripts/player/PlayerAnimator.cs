using UnityEngine;

public class PlayerAnimator : BaseBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform rayPosition;
    [SerializeField] private LayerMask borderLayer;
    private PlayerInputsReader inputs;
    private readonly int animatorMoveXHash = Animator.StringToHash("XMove"); 
    private readonly int animatorMoveZHash = Animator.StringToHash("ZMove"); 
    private readonly int animatorEndMove = Animator.StringToHash("EndMove"); 
    private readonly int animatorOnLoose = Animator.StringToHash("Loose");
    private readonly int animatorOnWin = Animator.StringToHash("Win");

    void OnEnable()
    {
        inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        animator = GetComponent<Animator>();

        GameManager.GetInstance().OnPlayerStateChanged += OnPlayerStateChanged;
        GameManager.GetInstance().OnGameStateChanged += OnGameStateChanged;
        inputs.OnMove += OnMove;
    }

    void OnPlayerStateChanged(PlayerState prev, PlayerState curr) {
        if (prev == PlayerState.MOVING && (curr is PlayerState.IDLE or PlayerState.TRAVELING)) {
            animator.SetTrigger(animatorEndMove);
            SetAnimatorValues();
        }
    }

    void OnGameStateChanged(GameState newState) {
        if (newState == GameState.VICTORY_ANIMATION) OnVictoryAnimation(); 
    }

    void OnDisable()
    {
        inputs.OnMove -= OnMove;
        GameManager.GetInstance().OnPlayerStateChanged -= OnPlayerStateChanged;
        GameManager.GetInstance().OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnMove(Vector3 v) {
        if (currentState != GameState.GAMEPLAY) return;
        if (controller.CurrentPlayerState != PlayerState.IDLE) return;
        if (Physics.Raycast(rayPosition.position, v, 1f, borderLayer)) return;
        SetAnimatorValues(v.x, v.z);
    }

    private void SetAnimatorValues(float x = 0, float z = 0) {
        animator.SetFloat(animatorMoveXHash, x);
        animator.SetFloat(animatorMoveZHash, z);
    }

    public void SetMoving() {
        controller.SetMovingState();
    }

    public void EndInitialAnimation() {
        controller.CurrentPlayerState = PlayerState.IDLE;
    }

    public void EndLooseAnimation() {
        controller.gameObject.SetActive(false);
        GameManager.GetInstance().OnLoose();
    }

    public void TriggerLoose()
    {
        controller.CurrentPlayerState = PlayerState.ANIMATION;
        animator.SetTrigger(animatorOnLoose);
    }

    private void OnVictoryAnimation()
    {
        controller.CurrentPlayerState = PlayerState.ANIMATION;
        animator.SetTrigger(animatorOnWin);
    }
    public void SetWin() {
        GameManager.GetInstance().OnWin();
    }
}
