using UnityEngine;
using UnityEngine.UI;
using System;

public class SapTimerUI : MonoBehaviour
{
    [Header("Cooldown UI & Gradient")]
    public Slider timerSlider;
    public Image checkmarkImage;
    public Image fillImage;
    public Gradient timerGradient;

    [Header("World Space Tracking")]
    public Vector3 offset = new Vector3(0, 2, 0); //Offset UI above the tree

    private float timer;
    private float timerDuration;
    private bool isCooldownActive = false;
    private Action onCooldownEnd;
    private Transform targetTransform;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (targetTransform != null)
        {
            UpdatePosition();
        }
    }

    void Update()
    {

        UpdatePosition();

        //Timer update logic
        if (!isCooldownActive) return;
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / timerDuration);
        timerSlider.value = progress;

        //Gradient color based on the timer progress, ranging from red -> orange -> yellow -> green
        if (fillImage != null)
        {
            fillImage.color = timerGradient.Evaluate(progress);
        }

        if (timer >= timerDuration)
        {
            isCooldownActive = false;
            onCooldownEnd?.Invoke();
            SetCheckmarkVisibility(true);
        }
    }

    public void Initialize(Transform target, bool isReadyToHarvest)
    {
        targetTransform = target;
        UpdatePosition();

        // Set the slider and checkmark based on tree state
        if (isReadyToHarvest)
        {
            SetSliderValue(1f);
            SetCheckmarkVisibility(true);
        }
        else
        {
            SetSliderValue(0f);
            SetCheckmarkVisibility(false);
        }
    }

    public void UpdatePosition()
    {
        if (mainCamera == null || targetTransform == null)
        {
            mainCamera = Camera.main; //Try to get camera if it's null
            if (mainCamera == null) return;
        }

        // Calculate world position above the tree
        Vector3 worldPosition = targetTransform.position + offset;

        // Convert to screen position - use this directly like in your working UI
        transform.position = mainCamera.WorldToScreenPoint(worldPosition);
    }

    public void StartCooldown(float duration, Action cooldownEndCallback)
    {
        timerDuration = duration;
        timer = 0;
        isCooldownActive = true;
        onCooldownEnd = cooldownEndCallback;
        SetCheckmarkVisibility(false);
    }

    public void SetSliderValue(float value)
    {
        timerSlider.value = value;
        if (fillImage != null)
        {
            fillImage.color = timerGradient.Evaluate(value);
        }
    }

    public void SetCheckmarkVisibility(bool show)
    {
        if (checkmarkImage != null)
        {
            checkmarkImage.gameObject.SetActive(show);
        }
    }

    public void SetUIVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}