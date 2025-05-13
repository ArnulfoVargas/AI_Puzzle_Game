using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCollider : MonoBehaviour
{
    [SerializeField] private SwipeStates moveDirection;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CollisionHandler ch)) {
            SwipeController.Instance.SetSwipeState(moveDirection);
        }
    }
}
