using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;

    [Header("Blur Effect")]
    public RawImage blurOverlay;
    public Material blurMaterial;
    [Range(0f, 20f)]
    public float blurIntensity = 3f;
    public float blurAnimationSpeed = 10f;

    private bool isPaused = false;
    private float currentBlurAmount = 0f;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(QuitToMenu);
        quitButton.onClick.AddListener(QuitGame);

        //Initialize blur overlay
        if (blurOverlay != null && blurMaterial != null)
        {
            //Create an instance of the material to avoid changing the original
            Material instancedBlurMaterial = new Material(blurMaterial);
            blurOverlay.material = instancedBlurMaterial;
            blurOverlay.gameObject.SetActive(false);
        }

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Animate blur effect
        if (blurOverlay != null && blurMaterial != null)
        {
            float targetBlur = isPaused ? blurIntensity : 0f;
            currentBlurAmount = Mathf.Lerp(currentBlurAmount, targetBlur, Time.unscaledDeltaTime * blurAnimationSpeed);

            // Apply blur amount to material
            blurOverlay.material.SetFloat("_BlurSize", currentBlurAmount);

            // Handle visibility
            if (isPaused || currentBlurAmount > 0.1f)
            {
                blurOverlay.gameObject.SetActive(true);

                // Update alpha for smoother transition
                Color overlayColor = blurOverlay.color;
                overlayColor.a = Mathf.Lerp(0, 0.7f, currentBlurAmount / blurIntensity);
                blurOverlay.color = overlayColor;
            }
            else
            {
                blurOverlay.gameObject.SetActive(false);
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
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
        if (isPaused)
        {
            TogglePause();
        }
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Time.timeScale = 1f;
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
}