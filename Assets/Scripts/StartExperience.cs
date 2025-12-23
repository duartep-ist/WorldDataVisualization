using Unity.XR.CoreUtils;
using UnityEngine;

public class StartExperience : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("Canvas ou Panel das instruções")]
    public GameObject instructionsCanvas;

    [Header("XR")]
    [Tooltip("XR Origin (XR Rig)")]
    public XROrigin xrOrigin;

    [Tooltip("Unidades para avançar para a frente")]
    public float forwardOffset = 1.5f;

    public void StartExperienceButton()
    {
        Debug.Log("START BUTTON FOI CARREGADO");
        FindObjectOfType<Logger>().Log("start_button_press", "");

        // 1. Esconder o painel
        if (instructionsCanvas != null)
            instructionsCanvas.SetActive(false);

        // 2. Avançar o XR Rig
        if (xrOrigin == null)
        {
            Debug.LogWarning("XR Origin não ligado");
            return;
        }

        Transform cameraTransform = xrOrigin.Camera.transform;

        // Direção para a frente baseada na cabeça (ignora Y)
        Vector3 forward = new Vector3(
            cameraTransform.forward.x,
            0f,
            cameraTransform.forward.z
        ).normalized;

        // Move o XR Origin
        xrOrigin.transform.position += forward * forwardOffset;
    }
}
