using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
public class Logger : MonoBehaviour
{
    private string logFilePath;
    private Transform cameraTransform;

    void Start()
    {
        // File path
        logFilePath = Path.Combine(Application.persistentDataPath, "VR_Log.txt");

        // Get the main camera (XR camera)
        cameraTransform = Camera.main.transform;

        // Create or clear log file
        File.WriteAllText(logFilePath, "timestamp,event_type,object_name\n");

        Log("app_start", "");
    }

    public void Log(string eventType, string objectName)
    {
        string time = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string final = $"{time},{eventType},{objectName}\n";
        File.AppendAllText(logFilePath, final, Encoding.UTF8);
    }
}
