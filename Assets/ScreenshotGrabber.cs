#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ScreenshotGrabber : MonoBehaviour
{
    public bool capture;

    private void OnValidate()
    {
        if (capture)
        {
            Grab();
        }
    }

    public static void Grab()
    {
        // Define the screenshots folder inside the Assets folder
        string screenshotFolder = Path.Combine(Application.dataPath, "Screenshots");

        // Ensure the folder exists
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
        }

        // Generate a unique file name using the current timestamp
        string fileName = "Screenshot_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // Combine the folder path and file name to get the full path
        string path = Path.Combine(screenshotFolder, fileName);

        // Capture the screenshot
        ScreenCapture.CaptureScreenshot(path);

        // Log the path where the screenshot is saved
        Debug.Log($"Screenshot saved to: {path}");
    }
}
#endif