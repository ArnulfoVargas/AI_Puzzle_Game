using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Configs/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public static AudioLibrary Library { get;  set; }
    public AudioFiles[] audioFiles;

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