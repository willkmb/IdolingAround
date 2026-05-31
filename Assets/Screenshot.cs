using UnityEngine;
using System.IO;

public class Screenshot : MonoBehaviour
{
    [SerializeField] private string folderPath = "Screenshots";
    [SerializeField, Range(1, 5)] private int size = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            string fileName = "screenshot_" + System.Guid.NewGuid() + ".png";
            string fullPath = Path.Combine(folderPath, fileName);

            ScreenCapture.CaptureScreenshot(fullPath, size);

            Debug.Log("Screenshot taken: " + fullPath);
        }
    }
}