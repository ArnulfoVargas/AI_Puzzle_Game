using System;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class MovingEnemy : EnemyBase {
    private float speed = 5;
    [SerializeField] private bool moveHorizontal, invert;
    private int moveFactor;
    private Rigidbody rb;
    Vector3 dir;

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + dir * ( speed * Time.fixedDeltaTime ));
    }
   

    protected override void OnStart()
    {
        moveFactor = invert ? -1 : 1; 
        dir = moveFactor * (moveHorizontal ? Vector3.right : Vector3.forward);
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) {
            dir *= -1;
        }
    } 
}