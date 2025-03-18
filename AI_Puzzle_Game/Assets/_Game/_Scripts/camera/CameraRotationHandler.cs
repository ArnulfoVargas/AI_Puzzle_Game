using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotationHandler : MonoBehaviour
{
    public float rotation;
    [SerializeField] Transform objectRotation;
    [SerializeField, Range(0.1f, 1.5f)] private float _rotationDuration = 0.75f;
    private Tweener rotTween;
    private GameManager manager;
    const int DEGREE_STEPS = 90;

    void Start()
    {
        rotation = 0;
        manager = GameManager.GetInstance();

        var inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        inputs.OnRotate += v => {
            if (GameManager.GetInstance().CurrentGameState != GameState.GAMEPLAY) return;

            var targetRot = rotation + (v * DEGREE_STEPS);
            var rot = new Vector3(0, targetRot, 0);

            if (rotTween.IsActive()) return;

            rotTween = objectRotation.DORotate(rot, .75f).OnComplete(() => {
                rotation = targetRot;
                manager.cameraRotation = rotation;
            });
        };
    }

    void OnDestroy()
    {
        rotTween.Kill();
    }
}
