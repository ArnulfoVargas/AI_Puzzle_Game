using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotationHandler : MonoBehaviour
{
    public static float Rotation {get; private set;} 
    [SerializeField] Slider slider;
    [SerializeField] Transform objectRotation;
    [SerializeField, Range(0.1f, 1.5f)] private float _rotationDuration = 0.75f;
    private Tweener rotTween;
    const int DEGREE_STEPS = 90;

    void Start()
    {
        slider.value = 0;
        Rotation = 0;
        slider.onValueChanged.AddListener((val) => {
            var targetRot = Mathf.Floor(val) * DEGREE_STEPS;
            var rot = new Vector3(0, targetRot, 0);

            if (rotTween.IsActive()) rotTween.Kill();

            rotTween = objectRotation.DORotate(rot, .75f).OnComplete(() => {
                Rotation = targetRot;
            });
        });
    }
}
