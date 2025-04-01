using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;

public class SapTimerUI : MonoBehaviour
{
    [Header("Cooldown UI & Gradient")]
    public Slider timerSlider;
    public Image checkmarkImage;
    public Image fillImage;
    public Gradient timerGradient;

    private float timer;
    private float timerDuration;
    private bool isCooldownActive = false;
    private Action onCooldownEnd;

    void Start()
    {
        if (checkmarkImage != null)
        {
            checkmarkImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
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
            checkmarkImage.gameObject.SetActive(true);
        }
    }

    public void StartCooldown(float duration, Action cooldownEndCallback)
    {
        timerDuration = duration;
        timer = 0;
        isCooldownActive = true;
        onCooldownEnd = cooldownEndCallback;
        checkmarkImage.gameObject.SetActive(false);
    }

    public void SetSliderValue(float value)
    {
        timerSlider.value = value;

        //Update slider color immediately based on the value
        if (fillImage != null)
        {
            fillImage.color = timerGradient.Evaluate(value);
        }
    }
}
