using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Utils;
using static MobileInputs;

[CreateAssetMenu(fileName = "PlayerInputsReader", menuName = "Inputs/PlayerInputsReader", order = 0)]
public class PlayerInputsReader : ScriptableObject, IPlayerActions
{
    [SerializeField, Range(.1f, 10)] private float sensibilityThreshold = 5;
    [SerializeField, Range(0.10f, 0.3f)] private double tapThreshold = 0.12f; 
    [SerializeField, Range(0.10f, 0.3f)] private float timeBetweenDoubleTap = 0.12f; 
    public On<Vector3> OnMove;
    public On<int> OnRotate; 
    const float MULTIPLIER = 100;
    bool shouldDetectInputs = true;
    [SerializeField]bool hasFirstTap = false;
    MobileInputs mobileInputs;
    Vector2 touchMovement;
    Vector3 moveDirection;
    Vector3 lastTouchPosition;
    private double startTime;
    private double firstTapTime;
    private int screenWidthHalf;

    void OnEnable()
    {
        mobileInputs ??= new();
        EnableInputs();
        screenWidthHalf = Mathf.RoundToInt(Screen.width * .5f);
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
        var touch = context.ReadValue<TouchState>();
        lastTouchPosition = touch.position;

        if (!shouldDetectInputs) return;
        if (context.performed) OnUpdatePosition(touch);
    }

    public void OnPrimaryTouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (hasFirstTap)
                if (Time.timeSinceLevelLoadAsDouble - firstTapTime > timeBetweenDoubleTap) hasFirstTap = false;

            if (GameManager.GetInstance().CurrentPlayerState == PlayerState.IDLE)
                touchMovement = Vector2.zero;
            else shouldDetectInputs = false;

            startTime = Time.timeSinceLevelLoadAsDouble;

            return;
        }
        else if (context.canceled)
        {
            var endTime = Time.timeSinceLevelLoadAsDouble - startTime;

            if (!CheckForTaps(endTime)) 
                if (shouldDetectInputs)
                    OnConfirmMovement();

            shouldDetectInputs = true;
        }
    }

    private bool CheckForTaps(double endTime) {
        if (endTime <= tapThreshold) {
            CompareTaps();
            return true;
        }
        return false;
    }

    private void CompareTaps() {
        if (hasFirstTap) {
            hasFirstTap = false;
            if (Time.timeSinceLevelLoadAsDouble - firstTapTime > timeBetweenDoubleTap) 
                return;
            CheckRotation();
            return;
        } 
        firstTapTime = Time.timeSinceLevelLoadAsDouble;
        hasFirstTap = true;
    }

    private void CheckRotation() {
        if (lastTouchPosition.x > screenWidthHalf) {
            OnRotate?.Invoke(-1);
        } else {
            OnRotate?.Invoke(1);
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