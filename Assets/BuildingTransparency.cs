using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingTransparency : MonoBehaviour
{
    [SerializeField] private List<Renderer> targetRenderers; // Drag and drop specific meshes here
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float targetAlpha = 0.3f;
    private Dictionary<Material, Color> originalColors = new Dictionary<Material, Color>();

    private void Start()
    {
        if (targetRenderers.Count == 0)
        {
            // Automatically find child renderers if not manually assigned
            targetRenderers.AddRange(GetComponentsInChildren<Renderer>());
        }

        // Store original colors
        foreach (Renderer rend in targetRenderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (!originalColors.ContainsKey(mat))
                {
                    originalColors[mat] = mat.color;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected! Fading materials...");

            StopAllCoroutines(); // Stop any ongoing fade
            StartCoroutine(FadeMaterials(targetAlpha));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exited trigger: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left! Restoring materials...");

            StopAllCoroutines();
            StartCoroutine(FadeMaterials(1f)); // Restore original opacity
        }
    }

    private IEnumerator FadeMaterials(float newAlpha)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            foreach (Renderer rend in targetRenderers)
            {
                foreach (Material mat in rend.materials)
                {
                    if (originalColors.ContainsKey(mat))
                    {
                        Color startColor = originalColors[mat];
                        mat.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(mat.color.a, newAlpha, t));
                    }
                }
            }

            yield return null;
        }

        // Ensure final transparency is applied
        foreach (Renderer rend in targetRenderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (originalColors.ContainsKey(mat))
                {
                    Color startColor = originalColors[mat];
                    mat.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
                }
            }
        }
    }
}
