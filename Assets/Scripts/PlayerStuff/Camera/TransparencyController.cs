using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // Assign the player in the inspector
    public Material transparencyMaterial;
    public float fadeRadius = 3f;
    public float fadeSmoothness = 1f;

    void Update()
    {
        if (transparencyMaterial)
        {
            transparencyMaterial.SetVector("_FadeCenter", player.position);
            transparencyMaterial.SetFloat("_FadeRadius", fadeRadius);
            transparencyMaterial.SetFloat("_FadeSmoothness", fadeSmoothness);
        }
    }
}
