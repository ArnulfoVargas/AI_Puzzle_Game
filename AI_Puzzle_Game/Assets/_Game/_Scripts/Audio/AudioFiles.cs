using System;
using UnityEngine;

[Serializable]
public class AudioFiles
{
    public Audio_Type audioType;
    public GameState[] playOn;
    [Range(-3, 3)]public float pitchMin = .95f, pitchMax = 1f;
    [Range(0,1)]public float volume = 1f;
    public AudioClip[] audioClips;

    public AudioClip GetRandomAudioClip()
    {
        if (audioClips.Length == 0) return null;
        return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    }
}