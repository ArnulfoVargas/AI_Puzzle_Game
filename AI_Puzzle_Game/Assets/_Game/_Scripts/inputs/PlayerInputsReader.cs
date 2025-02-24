using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Utils;
using static MobileInputs;

[CreateAssetMenu(fileName = "PlayerInputsReader", menuName = "Inputs/PlayerInputsReader", order = 0)]
public class PlayerInputsReader : ScriptableObject, IPlayerActions
{
    public On<Vector3> OnMove;
    MobileInputs mobileInputs;
    Vector2 touchMovement;
    Vector3 moveDirection;


    void OnEnable()
    {
        mobileInputs = new();
        mobileInputs.Player.Enable();
        mobileInputs.Player.SetCallbacks(this);
    }

    void OnDisable()
    {
        mobileInputs.Player.Disable();
    }


    public void OnPrimaryTouchMove(InputAction.CallbackContext context)
    {
        if (context.performed) OnUpdatePosition(context.ReadValue<TouchState>());
    }

    public void OnPrimaryTouch(InputAction.CallbackContext context)
    {
        if (context.started) touchMovement = Vector2.zero;
        else if (context.canceled) OnConfirmMovement();
    }

    private void OnUpdatePosition(TouchState touch) {
        touchMovement += touch.delta * 100;

        if (Vector2.Distance(this.touchMovement, Vector2.zero) < 500) return;

        if (Mathf.Abs(touchMovement.x) >= Mathf.Abs(touchMovement.y)) moveDirection = Vector3.right * (touchMovement.x >= 0 ? 1 : -1);
        else moveDirection = Vector3.forward * (touchMovement.y >= 0 ? 1 : -1);
    }

    private void OnConfirmMovement() {
        OnMove?.Invoke(Quaternion.AngleAxis(CameraRotationHandler.Rotation, Vector3.up) * moveDirection);
    }

    private void Print(object obj) => Debug.Log(obj);
}