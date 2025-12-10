using UnityEngine;

public class CountryBehavior : MonoBehaviour
{
    [Header("Settings")]
    public Color selectedColor = Color.cyan;

    private bool isSelected = false;
    private Color originalColor;
    private Vector3 originalScale;
    private Renderer meshRenderer;

    void Awake()
    {
        // Cache references and initial state
        meshRenderer = GetComponent<Renderer>();
        originalColor = meshRenderer.material.color;
        originalScale = transform.localScale;
    }

    // Call this to toggle selection state
    public void ToggleSelection()
    {
        isSelected = !isSelected;
        
        if (isSelected)
        {
            meshRenderer.material.color = selectedColor;
            // TODO: Call your UI Manager here to show info about this country
            Debug.Log($"Selected: {gameObject.name}");
        }
        else
        {
            meshRenderer.material.color = originalColor;
        }
    }

    // Call this to change height. 
    // heightFactor = 0.0f is surface level. 0.1f is 10% elevation.
    public void SetCountryHeight(float heightFactor)
    {
        // Since pivot is at (0,0,0), uniform scaling moves the surface radially.
        // Formula: NewScale = OriginalScale * (1 + height)
        transform.localScale = originalScale * (1.0f + heightFactor);
    }
}
