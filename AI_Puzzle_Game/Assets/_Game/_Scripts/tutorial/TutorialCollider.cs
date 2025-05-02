using System;
using UnityEngine;

public class TutorialCollider : BaseBehaviour, IInteractable {
    [SerializeField] TutorialHint tutorialData;
    protected virtual void OnTriggerEnter(Collider other){
        if (other.gameObject.TryGetComponent(out CollisionHandler ch)) {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        TutorialManager.GetInstance.SetDialogState(tutorialData);
    }
}
