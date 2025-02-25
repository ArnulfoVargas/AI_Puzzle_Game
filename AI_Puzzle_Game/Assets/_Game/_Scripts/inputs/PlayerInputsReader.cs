using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Utils;
using static MobileInputs;

[CreateAssetMenu(fileName = "PlayerInputsReader", menuName = "Inputs/PlayerInputsReader", order = 0)]
public class PlayerInputsReader : ScriptableObject, IPlayerActions
{
    [SerializeField, Range(.1f, 10)] private float sensibilityThreshold = 5;
    public On<Vector3> OnMove;
    const float MULTIPLIER = 100;
    bool shouldDetectInputs = true;
    MobileInputs mobileInputs;
    Vector2 touchMovement;
    Vector3 moveDirection;

    void OnEnable()
    {
        mobileInputs ??= new();
        EnableInputs();
    }

    public void EnableInputs()
    {
        mobileInputs.Player.Enable();
        mobileInputs.Player.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableInputs();
    }

    public void DisableInputs()
    {
        mobileInputs.Player.Disable();
    }

    public void OnPrimaryTouchMove(InputAction.CallbackContext context)
    {
        if (!shouldDetectInputs) return;
        if (context.performed) OnUpdatePosition(context.ReadValue<TouchState>());
    }

    public void OnPrimaryTouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameManager.GetInstance().CurrentPlayerState == PlayerState.IDLE)
                touchMovement = Vector2.zero;
            else shouldDetectInputs = false;

            return;
        }
        else if (context.canceled)
        {
            if (shouldDetectInputs)
                OnConfirmMovement();

            shouldDetectInputs = true;
        }
    }

    private void OnUpdatePosition(TouchState touch)
    {
        touchMovement += touch.delta * MULTIPLIER;

        if (Vector2.Distance(this.touchMovement, Vector2.zero) < sensibilityThreshold * MULTIPLIER) return;

        if (Mathf.Abs(touchMovement.x) >= Mathf.Abs(touchMovement.y)) moveDirection = Vector3.right * (touchMovement.x >= 0 ? 1 : -1);
        else moveDirection = Vector3.forward * (touchMovement.y >= 0 ? 1 : -1);
    }

    private void OnConfirmMovement()
    {
        OnMove?.Invoke(Quaternion.AngleAxis(GameManager.GetInstance().cameraRotation, Vector3.up) * moveDirection);
    }

    private void Print(object obj) => Debug.Log(obj);
}