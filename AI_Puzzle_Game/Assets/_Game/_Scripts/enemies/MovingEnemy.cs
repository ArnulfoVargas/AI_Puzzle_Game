using UnityEngine;

public class MovingEnemy : EnemyBase {
    [SerializeField] private Vector2 moveDirection;
    private Vector3 dir;
    private int moveModifier = 1;
    private float speed = 5f;

    protected override void OnStart()
    {
        base.OnStart();
        dir = new Vector3(moveDirection.x, 0, moveDirection.y).normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            moveModifier *= -1;
    }

    protected override void OnGameplayUpdate()
    {
        transform.Translate(dir * (Time.deltaTime * moveModifier * speed));
    }
}