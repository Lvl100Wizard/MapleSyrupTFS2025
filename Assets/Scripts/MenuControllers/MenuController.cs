using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using static UnityEditorInternal.ReorderableList;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button applySettingsButton;
    public Button backButton;
    public Button fullscreenButton;

    [Header("Audio Settings")]
    public AudioMixer audioMixer;

    [Header("Fullscreen Toggle")]
    public GameObject fullscreenIconOn;
    public GameObject fullscreenIconOff;

    private bool isFullscreen = false;

    void Start()
    {
        // Assign button actions
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        applySettingsButton.onClick.AddListener(ApplySettings);
        backButton.onClick.AddListener(BackToMenu);
        fullscreenButton.onClick.AddListener(ToggleFullscreen);

        // Initialize Volume Slider
        InitializeVolumeSettings();
    }

    void InitializeVolumeSettings()
    {
        // Load saved volume setting, or use default if not set
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedVolume) * 20); // Apply to AudioMixer
        volumeTextValue.text = (savedVolume * 100).ToString("0") + "%"; // Show percentage

        // Update volume text and volume when slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // Update the audio volume
        AudioListener.volume = volume;

        // Update the AudioMixer volume if you're using one
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        // Update the volume text with percentage format
        volumeTextValue.text = (volume * 100).ToString("0") + "%";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void BackToMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;

        Screen.fullScreen = isFullscreen;

        fullscreenIconOn.SetActive(isFullscreen);
        fullscreenIconOff.SetActive(!isFullscreen);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game called");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  // Quits the game when it’s a built executable
#endif
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}
