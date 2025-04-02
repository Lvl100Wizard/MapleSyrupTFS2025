using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;

    //[Header("Volume Settings")]
    //[SerializeField] private TMP_Text volumeTextValue = null;
    //[SerializeField] private Slider volumeSlider = null;
    //[SerializeField] private float defaultVolume = 1.0f;

    [Header("MainMenu Buttons")]
    public Button playButton;
    public Button howToPlayButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("HowToPlay Buttons")]
    public Button howToPlayBackButton;

    [Header("Settings Buttons")]
    public Button applySettingsButton;
    public Button settingsBackButton;

    //[Header("Audio Settings")]
    //public AudioMixer audioMixer;

    private Stack<GameObject> panelHistory = new Stack<GameObject>();

    void Start()
    {
        // Assign button actions
        playButton.onClick.AddListener(StartGame);
        howToPlayButton.onClick.AddListener(() => OpenPanel(howToPlayPanel));
        settingsButton.onClick.AddListener(() => OpenPanel(settingsPanel));
        quitButton.onClick.AddListener(QuitGame);
        howToPlayBackButton.onClick.AddListener(BackToPreviousPanel);
        //applySettingsButton.onClick.AddListener(ApplySettings);
        settingsBackButton.onClick.AddListener(BackToPreviousPanel);

        //InitializeVolumeSettings();
    }

    //void InitializeVolumeSettings()
    //{
    //    float savedVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
    //    volumeSlider.value = savedVolume;
    //    audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedVolume) * 20);
    //    volumeTextValue.text = (savedVolume * 100).ToString("0") + "%";
    //    volumeSlider.onValueChanged.AddListener(SetVolume);
    //}

    //public void SetVolume(float volume)
    //{
    //    audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    //    volumeTextValue.text = (volume * 100).ToString("0") + "%";
    //}

    public void StartGame()
    {
        SceneManager.LoadScene("TestScene_Jacob");
    }

    public void OpenPanel(GameObject panelToOpen)
    {
        if (panelToOpen.activeSelf) return;

        if (panelHistory.Count == 0 || panelHistory.Peek() != panelToOpen)
        {
            panelHistory.Push(GetActivePanel());
        }

        GetActivePanel()?.SetActive(false);
        panelToOpen.SetActive(true);
    }

    public void BackToPreviousPanel()
    {
        if (panelHistory.Count > 0)
        {
            GameObject previousPanel = panelHistory.Pop();
            GetActivePanel()?.SetActive(false);
            previousPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game called");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //public void ApplySettings()
    //{
    //    PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
    //    PlayerPrefs.Save();
    //}

    private GameObject GetActivePanel()
    {
        if (mainMenuPanel.activeSelf) return mainMenuPanel;
        if (howToPlayPanel.activeSelf) return howToPlayPanel;
        if (settingsPanel.activeSelf) return settingsPanel;
        return null;
    }
}
