using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingEnemy : EnemyBase
{
    [SerializeField] private GameObject visual;
    [SerializeField] private float hideTime = 2; 
    [SerializeField] private float damageTime = 2;
    [SerializeField] private bool isHided;
    private float waitTime;
    private BoxCollider col;
    private float t = 0;

    protected override void OnStart()
    {
        base.OnStart();

        col = GetComponent<BoxCollider>();

        UpdateHided();
    }

    private void UpdateHided() {
        col.enabled = !isHided;
        visual.SetActive(!isHided);
        waitTime = isHided ? hideTime : damageTime;
    }

    protected override void OnGameplayUpdate()
    {
        t += Time.deltaTime;

        if (t >= waitTime) {
            isHided = !isHided;
            UpdateHided();
            t = 0;
        }
    }
}
