using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Configs/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public static AudioLibrary Library { get;  set; }
    [SerializeField] public AudioFiles[] audioFiles;

    public AudioClip GetRandomFromType(Audio_Type audioType, out AudioFiles audioFile)
    {
        for (int i = 0; i < audioFiles.Length; i++)
        {
            if (audioFiles[i].audioType == audioType)
            {
                audioFile = audioFiles[i];
                return audioFiles[i].GetRandomAudioClip();
            }
        }

        Debug.LogError($"Audio type {audioType} not found in AudioLibrary.");
        audioFile = null;
        return null;
    }

    public AudioClip GetRandomFromType(Audio_Type audioType)
    {
        for (int i = 0; i < audioFiles.Length; i++)
        {
            if (audioFiles[i].audioType == audioType)
            {
                return audioFiles[i].GetRandomAudioClip();
            }
        }

        Debug.LogError($"Audio type {audioType} not found in AudioLibrary.");
        return null;
    }
}