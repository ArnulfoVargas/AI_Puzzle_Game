using System;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class MovingEnemy : EnemyBase {
    [SerializeField] bool moveHorizontal, invert;
    [SerializeField] private Vector3 dir;
    [SerializeField] private float secondsPerMeter = 0.5f;
    private int moveModifier = 1;
    private Tweener tweener;

    protected override void OnStart()
    {
        base.OnStart();
        moveModifier = invert ? -1 : 1;
        Move();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) {
            if (tweener.active) tweener.Kill();
            var p = transform.position;
            transform.position = new Vector3(Mathf.RoundToInt(p.x), 0, Mathf.RoundToInt(p.z));
            moveModifier *= -1;
            Move();
        }
    }

    private void Move()
    {
        if (moveHorizontal) tweener = transform.DOMoveX(transform.position.x + moveModifier, secondsPerMeter).SetEase(Ease.Linear).OnComplete(Move);
        else tweener = transform.DOMoveZ(transform.position.z + moveModifier, secondsPerMeter).SetEase(Ease.Linear).OnComplete(Move);
    }
}