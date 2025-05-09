using System;
using System.Linq;
using UnityEngine;

public class AudioController : BaseBehaviour {
    private GameState[] playOn;
    public AudioSource audioSource;

    public void UpdatePlayOn(GameState[] states) {
        playOn = states;
        Verify(currentState);
    }
    protected override void OnUpdateState(GameState state)
    {
        Debug.Log(state);
        Verify(state);
    }

    private void Verify(GameState state) {
        if (playOn.Contains(state)) {
            audioSource.UnPause();
        } else {
            audioSource.Pause();
        }
    }
}