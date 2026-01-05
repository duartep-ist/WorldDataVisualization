using System.Buffers;
using UnityEngine;

public class CountryBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public Color selectedColor = Color.cyan;

    private Color originalColor;
    private Renderer meshRenderer;

    private Manager manager;

    void Start()
    {
        // Cache references and initial state
        meshRenderer = GetComponent<Renderer>();
        originalColor = meshRenderer.material.color;
        manager = FindObjectOfType<Manager>();
    }

    public string GetIsoA3Code()
    {
        return gameObject.name;
    }

    public void Select()
    {
        meshRenderer.material.color = selectedColor;
    }
    public void Deselect()
    {
        meshRenderer.material.color = originalColor;
    }

    // Height factor range: 0.0f to 1.0f
    public void SetHeight(float heightFactor)
    {
        // Since pivot is at (0,0,0), uniform scaling moves the surface radially.
        transform.localScale = Vector3.one * (1.075f + 0.25f * heightFactor);
    }
}
