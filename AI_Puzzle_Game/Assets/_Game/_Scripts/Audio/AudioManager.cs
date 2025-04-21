using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseBehaviour
{
    public static AudioManager Instance { get; set;}

    public AudioLibrary audioLibrary;
    public static AudioManager GetInstance() { return Instance; }

    void Awake()
    {
        AudioLibrary.Library = audioLibrary;
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    AudioSource audioSource;
    [SerializeField] private GameObject audioSorucePrefab;
    List<AudioSource> audioSourcePool = new List<AudioSource>();

    public void SetAudio(AudioClip _clip)
    {
        audioSource.PlayOneShot(_clip);
    }
    public void SetAudio(Audio_Type audioType)
    {
        audioSource.PlayOneShot(AudioLibrary.Library.GetRandomFromType(audioType));
    }

    public void SetAudioWithPosition(AudioClip _clip, Vector3 _position)
    {
        AudioSource newAudio = GetAudioSource();
        newAudio.transform.position = _position;
        newAudio.clip = _clip;
        newAudio.Play();
        newAudio.GetComponent<AutoOff>().SetNewTime(_clip.length);
    }
    public void SetAudioWithPosition(Audio_Type type, Vector3 _position)
    {
        SetAudioWithPosition(AudioLibrary.Library.GetRandomFromType(type), _position);
    }

    AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSourcePool.Count; i++)
        {
            if (!audioSourcePool[i].gameObject.activeInHierarchy)
            {
                audioSourcePool[i].gameObject.SetActive(true);
                return audioSourcePool[i];
            }
        }

        AudioSource newAudio = Instantiate(audioSorucePrefab).GetComponent<AudioSource>();
        newAudio.transform.parent = transform;
        audioSourcePool.Add(newAudio);
        return newAudio;
    }
}