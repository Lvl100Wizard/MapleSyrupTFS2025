using UnityEngine;
using System.IO;

public class SimpleIconCapture : MonoBehaviour
{
    public int resolution = 512; // Icon size

    void Start()
    {
        TakeScreenshot();
    }

    void TakeScreenshot()
    {
        // Create a Render Texture
        RenderTexture rt = new RenderTexture(resolution, resolution, 24);
        GetComponent<Camera>().targetTexture = rt;

        // Capture the frame
        Texture2D screenshot = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        screenshot.Apply();

        // Save to file
        byte[] bytes = screenshot.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, "ModelIcon.png");
        File.WriteAllBytes(path, bytes);

        // Cleanup
        GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Debug.Log("Icon saved at: " + path);
    }
}
