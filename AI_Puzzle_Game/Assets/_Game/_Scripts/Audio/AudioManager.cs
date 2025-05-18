using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : BaseBehaviour
{
    public static AudioManager Instance { get; set;}

    public AudioLibrary audioLibrary;
    public static AudioManager GetInstance() { return Instance; }
    [SerializeField] private AudioSource ambientMusicAudioSource;
    [SerializeField] private AudioClip mainMenuAudioClip, inGameAudioClip;
    AudioSource audioSource;
    [SerializeField] private GameObject audioSorucePrefab;
    List<AudioSource> audioSourcePool = new List<AudioSource>();

    void Awake()
    {
        AudioLibrary.Library = audioLibrary;
        Instance ??= this;
        if (Instance != this) {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch (scene.buildIndex) {
            case 0:
                ambientMusicAudioSource.Stop();
                break;
            case 1:
                if (ambientMusicAudioSource.clip != mainMenuAudioClip) {
                    ambientMusicAudioSource.Stop();
                    ambientMusicAudioSource.clip = mainMenuAudioClip;
                    ambientMusicAudioSource.Play();
                }
                break;
            default:
                if (ambientMusicAudioSource.clip != inGameAudioClip) {
                    ambientMusicAudioSource.Stop();
                    ambientMusicAudioSource.clip = inGameAudioClip;
                    ambientMusicAudioSource.Play();
                }
                break;
        }
    }

    public void SetAudio(AudioClip _clip)
    {
        audioSource.PlayOneShot(_clip);
    }
    public void SetAudio(Audio_Type audioType)
    {
        audioSource.PlayOneShot(AudioLibrary.Library.GetRandomFromType(audioType));
    }

    public void PlayUiAudio() {
        SetAudio(Audio_Type.CLICK);
    }

    public void SetAudioWithPosition(Audio_Type type, Vector3 _position)
    {
        var _clip = AudioLibrary.Library.GetRandomFromType(type, out AudioFiles file);
        var pitch = Random.Range(file.pitchMin, file.pitchMax);
        AudioSource newAudio = GetAudioSource();
        newAudio.transform.position = _position;
        newAudio.pitch = pitch;
        newAudio.clip = _clip;
        newAudio.volume = file.volume;
        newAudio.Play();
        newAudio.GetComponent<IAutoOff>().SetNewTime(_clip.length * (1 / pitch));
        newAudio.GetComponent<AudioController>().UpdatePlayOn(file.playOn);
    }

    public void SetAudioWithZeroPosition(Audio_Type type) {
        SetAudioWithPosition(type, Vector3.zero);
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