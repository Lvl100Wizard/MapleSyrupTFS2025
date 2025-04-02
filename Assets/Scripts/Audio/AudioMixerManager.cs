using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager instance;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        // Ensure there's only one instance of AudioMixerManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    // Methods that only update the mixer without saving to PlayerPrefs
    public void UpdateMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }

    public void UpdateSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20);
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    // Methods that save to PlayerPrefs and update the mixer
    public void SetMasterVolume(float volume)
    {
        UpdateMasterVolume(volume);
        PlayerPrefs.SetFloat("masterVolume", volume); // Save volume setting
    }

    public void SetSoundFXVolume(float volume)
    {
        UpdateSoundFXVolume(volume);
        PlayerPrefs.SetFloat("soundFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        UpdateMusicVolume(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    // Apply all saved settings at once
    public void ApplySavedSettings()
    {
        UpdateMasterVolume(PlayerPrefs.GetFloat("masterVolume", 1.0f));
        UpdateMusicVolume(PlayerPrefs.GetFloat("musicVolume", 1.0f));
        UpdateSoundFXVolume(PlayerPrefs.GetFloat("soundFXVolume", 1.0f));
    }

    private void Start()
    {
        // Load saved volume settings when the game starts
        ApplySavedSettings();
    }
}