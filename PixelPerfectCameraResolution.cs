using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelPerfectCameraResolution : MonoBehaviour
{
    public PixelPerfectCamera pixelPerfectCamera;

    void Start()
    {
        int height = Screen.height / 5;
        int width = Screen.width / 5;

        if (height % 2 != 0) height++;
        if (width % 2 != 0) width++;

        pixelPerfectCamera.refResolutionY = height;
        pixelPerfectCamera.refResolutionX = width;
    }
}