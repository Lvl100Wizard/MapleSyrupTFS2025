using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PauseController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button quitToMenuButton;

    void Start()
    {
        // Assign button actions
        resumeButton.onClick.AddListener(ResumeGame);
        quitToMenuButton.onClick.AddListener(QuitToMenu);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = !ppVolume.enabled;
        
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        PauseGame();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Time.timeScale = 1f;
    }
}
