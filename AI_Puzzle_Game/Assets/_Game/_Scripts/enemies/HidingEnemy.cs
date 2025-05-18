using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingEnemy : EnemyBase
{
    [SerializeField] private GameObject visual;
    [SerializeField] private float hideTime = 2; 
    [SerializeField] private float damageTime = 2;
    [SerializeField] private bool isHided;
    [SerializeField] private AnimationCurve hidingCurve;
    [SerializeField] private AnimationCurve showingCurve;
    private float curvesSpeed = 1.15f;
    private AnimationCurve curve;
    private float waitTime;
    private BoxCollider col;
    private float t = 0;
    private float curveT;
    private bool animating;
    [SerializeField] private MeshRenderer mRenderer;
    [SerializeField] private ParticleSystemRenderer _particleSystem;
    [SerializeField] private SpriteRenderer _renderer;

    protected override void OnStart()
    {
        base.OnStart();

        col = GetComponent<BoxCollider>();
        mRenderer.material.SetFloat("_Alpha", isHided ? 0 : 1);
        UpdateHided();
    }

    public void UpdateHided() {
        col.enabled = !isHided;
        waitTime = isHided ? hideTime : damageTime;
        animating = false;
        t = 0;
    }

    protected override void OnGameplayUpdate()
    {
        t += Time.fixedDeltaTime;

        if (t >= waitTime && !animating) {
            isHided = !isHided;
            curveT = 0;
            animating = true;
            curve = isHided ? hidingCurve : showingCurve;
        }

        if (!animating) return;
        curveT += Time.fixedDeltaTime * curvesSpeed;
        var cVal = curve.Evaluate(curveT);
        mRenderer.material.SetFloat("_Alpha", cVal);
        _particleSystem.material.color = new Color(1, 1, 1, cVal);
        _renderer.color = new Color(1, 0, 0, 0.8352941f * cVal);

        if (curveT >= 1) {
            animating = false;
            UpdateHided();
        }
    }
}
