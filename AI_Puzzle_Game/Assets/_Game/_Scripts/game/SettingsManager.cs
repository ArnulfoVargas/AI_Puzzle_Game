using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : BaseBehaviour {
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider MasterVolume, SoundEffectsVolume, MusicVolume;
    SoundConfig config;

    protected override void OnStart()
    {
        var loadedConf = ES3.Load("sound_config", new SoundConfig{
            MasterVolume = 1,
            MusicVolume = 1,
            SfxVolume = 1
        });

        UpdateSfxVolume(loadedConf.SfxVolume);
        UpdateMusicVolume(loadedConf.MusicVolume);
        UpdateMasterVolume(loadedConf.MasterVolume);

        SoundEffectsVolume.value = config.SfxVolume;
        MusicVolume.value = config.MusicVolume;
        MasterVolume.value = config.MasterVolume;
    }

    void OnEnable()
    {
        SoundEffectsVolume.onValueChanged.AddListener(UpdateSfxVolume);
        MusicVolume.onValueChanged.AddListener(UpdateMusicVolume);
        MasterVolume.onValueChanged.AddListener(UpdateMasterVolume);
    }

    private void OnDestroy() {
        SoundEffectsVolume.onValueChanged.RemoveListener(UpdateSfxVolume);
        MusicVolume.onValueChanged.RemoveListener(UpdateMusicVolume);
        MasterVolume.onValueChanged.RemoveListener(UpdateMasterVolume);
    }

    void UpdateSfxVolume(float value)
    {
        config.SfxVolume = CalculateValueClamped(value);
        mixer.SetFloat("SFX", CalculateVolume(config.SfxVolume));
    } 
    void UpdateMusicVolume(float value)
    {
        config.MusicVolume = CalculateValueClamped(value);
        mixer.SetFloat("Music", CalculateVolume(config.MusicVolume));
    } 

    void UpdateMasterVolume(float value)
    {
        config.MasterVolume = CalculateValueClamped(value);
        mixer.SetFloat("Master", CalculateVolume(config.MasterVolume));
    } 

    float CalculateVolume(float value) {
        return Mathf.Log10(value) * 20;
    }

    float CalculateValueClamped(float value) {
        return Mathf.Clamp(value, 0.0001f, 1);
    }

    public void Save() {
        ES3.Save("sound_config", config);
    }
}