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

    // Height factor range: 0.0f to 1.0f
    public void SetHeight(float heightFactor)
    {
        // Since pivot is at (0,0,0), uniform scaling moves the surface radially.
        transform.localScale = Vector3.one * (1.075f + 0.25f * heightFactor);
    }
}
