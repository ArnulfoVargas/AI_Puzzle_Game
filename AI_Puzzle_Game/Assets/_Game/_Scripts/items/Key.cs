using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : BaseBehaviour, IInteractable
{
    [SerializeField] UnityEvent OnCollectKey;
    [SerializeField] Collider col;
    
    protected virtual void OnTriggerEnter(Collider other){
        if (other.gameObject.TryGetComponent(out CollisionHandler ch)) {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        col.enabled = false;
        OnCollectKey?.Invoke();
    }
}
