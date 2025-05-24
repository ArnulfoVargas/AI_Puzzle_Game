using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraRotationHandler : BaseBehaviour
{
    public float rotation;
    [SerializeField] Transform objectRotation;
    // [SerializeField, Range(0.1f, 1.5f)] private float _rotationDuration = 0.75f;
    [SerializeField] AnimationCurve rotationCurve;
    [SerializeField] private float rotationSpeed;
    private Tweener rotTween;
    private bool isRotating = false;
    const int DEGREE_STEPS = 90;
    private float rotatingTime;
    private float finalRotation;

    protected override void OnStart()
    {
        rotation = 0;
        manager = GameManager.GetInstance();

        var inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        inputs.OnRotate += OnRotate;
    }

    protected override void OnUpdateState(GameState state)
    {
        if (state != GameState.GAMEPLAY)
            if (isRotating)
                objectRotation.DOPause();
        else
            if (isRotating)
                objectRotation.DOPlay();
    }

    void OnRotate(int v) {
        if (GameManager.GetInstance().CurrentGameState != GameState.GAMEPLAY) return;
        if (isRotating) return;

        var targetRot = rotation + (v * DEGREE_STEPS);
        finalRotation = targetRot;

        rotatingTime = 0;
        isRotating = true;
    }

    protected override void OnGameplayUpdate()
    {
        if (isRotating)
        {
            rotatingTime += Time.fixedDeltaTime * rotationSpeed;
            objectRotation.rotation = Quaternion.Euler(0, Mathf.LerpAngle(rotation, finalRotation, rotationCurve.Evaluate(rotatingTime)), 0);

            if (rotatingTime >= 1) {
                isRotating = false;
                manager.cameraRotation = finalRotation;
                rotation = finalRotation;
            }
        }
    }

    void OnDestroy()
    {
        rotTween.Kill();
    }
}
