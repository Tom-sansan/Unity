using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AspectKeeper Class
/// </summary>
[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    /// <summary>
    /// Target camera
    /// </summary>
    [SerializeField]
    private Camera targetCamera;
    /// <summary>
    /// Target resolution
    /// </summary>
    [SerializeField]
    private Vector2 aspectVec;

    void Update()
    {
        // Screen aspect ratio
        var screenAspect = Screen.width / (float)Screen.height;
        // Target aspect ratio
        var targetAspect = aspectVec.x / aspectVec.y;
        // Magnification to target aspect ratio
        var magRate = targetAspect / screenAspect;
        // Create Rect with Viewport initial values
        var viewportRect = new Rect(0, 0, 1, 1);
        if (magRate < 1)
        {
            // Change the width to use
            viewportRect.width = magRate;
            // Centering
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            // Change the height to use
            viewportRect.height = 1 / magRate;
            // Centering
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }
        // Apply to camera Viewport
        targetCamera.rect = viewportRect;
    }
}
