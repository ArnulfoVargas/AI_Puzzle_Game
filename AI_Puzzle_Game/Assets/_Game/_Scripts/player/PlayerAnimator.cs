using UnityEngine;

public class PlayerAnimator : BaseBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController controller;
    private PlayerInputsReader inputs;
    private readonly int animatorMoveXHash = Animator.StringToHash("XMove"); 
    private readonly int animatorMoveZHash = Animator.StringToHash("ZMove"); 
    private readonly int animatorEndMove = Animator.StringToHash("EndMove"); 

    void OnEnable()
    {
        inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        animator = GetComponent<Animator>();

        GameManager.GetInstance().OnPlayerStateChanged += OnPlayerStateChanged;
        inputs.OnMove += OnMove;
    }

    void OnPlayerStateChanged(PlayerState prev, PlayerState curr) {
        if (prev == PlayerState.MOVING && curr == PlayerState.IDLE) {
            animator.SetTrigger(animatorEndMove);
            SetAnimatorValues();
        }
    }

    void OnDisable()
    {
        inputs.OnMove -= OnMove;
        GameManager.GetInstance().OnPlayerStateChanged -= OnPlayerStateChanged;
    }

    private void OnMove(Vector3 v) {
        SetAnimatorValues(v.x, v.z);
    }

    private void SetAnimatorValues(float x = 0, float z = 0) {
        animator.SetFloat(animatorMoveXHash, x);
        animator.SetFloat(animatorMoveZHash, z);
    }

    public void SetMoving() {
        controller.SetMovingState();
    }
}
