using System.Collections.Generic;
using UnityEngine;

public class ParticlesStopper : BaseBehaviour {
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private List<GameState> playOnStates;

    protected override void OnUpdateState(GameState state)
    {
        if (playOnStates.Contains(state)) {
            _particleSystem.Play();
        } else {
            _particleSystem.Pause();
        }
    }
}