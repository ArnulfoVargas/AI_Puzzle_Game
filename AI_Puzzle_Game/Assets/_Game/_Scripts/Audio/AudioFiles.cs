using System;
using UnityEngine;

[Serializable]
public class AudioFiles
{
    public Audio_Type audioType;
    public GameState PlayOn;
    public AudioClip[] audioClips;

    public AudioClip GetRandomAudioClip()
    {
        if (audioClips.Length == 0) return null;
        return audioClips[UnityEngine.Random.Range(0, audioClips.Length + 1)];
    }
}