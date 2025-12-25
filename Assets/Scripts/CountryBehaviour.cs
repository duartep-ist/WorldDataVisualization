using System.Buffers;
using UnityEngine;

public class CountryBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public Color selectedColor = Color.cyan;

    private Color originalColor;
    private Vector3 originalScale;
    private Renderer meshRenderer;

    private Manager manager;

    void Start()
    {
        // Cache references and initial state
        meshRenderer = GetComponent<Renderer>();
        originalColor = meshRenderer.material.color;
        originalScale = transform.localScale;
        manager = FindObjectOfType<Manager>();
    }

    public int GetCountryID()
    {
        string countryName = gameObject.name;
        if (countryName == "ne_110m_admin_0_countries")
        {
            return 0;
        }
        if (int.TryParse(countryName.Split('.')[1], out int id))
        {
            return id;
        }
        throw new System.ArgumentException($"Invalid country name format: {countryName}");
    }

    public void Select()
    {
        meshRenderer.material.color = selectedColor;
    }
    public void Deselect()
    {
        meshRenderer.material.color = originalColor;
    }

    // heightFactor = 0.0f is surface level. 0.1f is 10% elevation.
    public void SetHeight(float heightFactor)
    {
        // Since pivot is at (0,0,0), uniform scaling moves the surface radially.
        transform.localScale = originalScale * (1.0f + heightFactor);
    }
}
