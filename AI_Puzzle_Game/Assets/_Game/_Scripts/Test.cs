using UnityEngine;

public class Test : EnemyBase
{
    [SerializeField] private float speed;
    [SerializeField] private bool moveHorizontal, invert;
    private int moveFactor;
    Vector3 dir;

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);       
    }

    protected override void OnStart()
    {
        moveFactor = invert ? -1 : 1; 
        dir = moveFactor * (moveHorizontal ? Vector3.right : Vector3.forward);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) {
            dir *= -1;
        }
    }
}
