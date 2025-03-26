using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorStopper : BaseBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float defaultSpeed = 1;
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdateState(GameState state)
    {
        if (state == GameState.GAMEPLAY) {
            animator.speed = defaultSpeed;
        } else {
            animator.speed = 0;
        } 
    }
}
