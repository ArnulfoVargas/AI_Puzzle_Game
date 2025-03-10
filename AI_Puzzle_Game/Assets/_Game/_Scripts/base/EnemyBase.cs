using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyBase : BaseBehaviour {
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable)) {
            damageable.Damage();
        }
    }
}