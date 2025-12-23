using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class CountryInteractable : XRBaseInteractable
{
    public int selectedCountries = 0;
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        Debug.Log($"Country clicked {gameObject.name}");

        FindObjectOfType<Logger>().Log("country_select", gameObject.name);

        if (gameObject.name == "ne_110m_admin_0_countries.215" )
        {
            selectedCountries = 215;
            ToggleCubes();
            
        }

        if (gameObject.name == "ne_110m_admin_0_countries.005")
        {
            selectedCountries = 5;
            ToggleCubes();
        }

        if (gameObject.name == "ne_110m_admin_0_countries.258" )
        {
            selectedCountries = 258;
            ToggleCubes();
        }

        if ( gameObject.name == "ne_110m_admin_0_countries.195")
        {
            selectedCountries = 195;
            ToggleCubes();
        }

        CountryBehavior countryBehavior = GetComponent<CountryBehavior>();
        if (countryBehavior != null)
        {
            countryBehavior.ToggleSelection();
        }
    }

    void ToggleCubes()
    {
        GameObject[] cubes = new GameObject[20];
        if (selectedCountries == 215) { 
            cubes = GameObject.FindGameObjectsWithTag("Cube");
        }
        if (selectedCountries == 5)
        {
             cubes = GameObject.FindGameObjectsWithTag("Cube2");
        }
        if (selectedCountries == 258)
        {
            cubes = GameObject.FindGameObjectsWithTag("Cube3");
        }
        if (selectedCountries == 195)
        {
            cubes = GameObject.FindGameObjectsWithTag("Cube4");
        }

        foreach (GameObject cube in cubes)
        {
            
            float targetHeight = GetHeightForYear(cube.name);

            // Se já estiver levantado, baixa; se estiver em 0, sobe
            Vector3 scale = cube.transform.localScale;
            scale.y = Mathf.Approximately(scale.y, 0f) ? targetHeight : 0f;
            cube.transform.localScale = scale;
        }
    }

    float GetHeightForYear(string year)
    {
       
        switch (year)
        {
            case "2020_1": return 23f;
            case "2020_2": return 8f;
            case "2020_3": return 12f;
            case "2020_4": return 52f;
            case "2021_1": return 10f;
            case "2021_2": return 28f;
            case "2021_3": return 32f;
            case "2021_4": return 42f;
            case "2022_1": return 80f;
            case "2022_2": return 12f;
            case "2022_3": return 21f;
            case "2022_4": return 42f;
            case "2023_1": return 15f;
            case "2023_2": return 15f;
            case "2023_3": return 15f;
            case "2023_4": return 15f;
            case "2024_1": return 36f;
            case "2024_2": return 36f;
            case "2024_3": return 57f;
            case "2024_4": return 100f;
         
            default: return 0f;
        }
    }
}
