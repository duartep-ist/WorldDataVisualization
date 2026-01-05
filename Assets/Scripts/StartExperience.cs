using Unity.XR.CoreUtils;
using UnityEngine;

public class StartExperience : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("Canvas ou Panel das instru��es")]
    public GameObject instructionsCanvas;

    [Header("XR")]
    [Tooltip("XR Origin (XR Rig)")]
    public XROrigin xrOrigin;

    [Tooltip("Unidades para avan�ar para a frente")]
    public float forwardOffset = 1.5f;

    public void StartExperienceButton()
    {
        Debug.Log("START BUTTON FOI CARREGADO");
        FindFirstObjectByType<Logger>().Log("start_button_press", "");

        // 1. Esconder o painel
        if (instructionsCanvas != null)
            instructionsCanvas.SetActive(false);

        // 2. Avan�ar o XR Rig
        if (xrOrigin == null)
        {
            Debug.LogWarning("XR Origin n�o ligado");
            return;
        }

        Transform cameraTransform = xrOrigin.Camera.transform;

        // Dire��o para a frente baseada na cabe�a (ignora Y)
        Vector3 forward = new Vector3(
            cameraTransform.forward.x,
            0f,
            cameraTransform.forward.z
        ).normalized;

        // Move o XR Origin
        xrOrigin.transform.position += forward * forwardOffset;
    }
}
