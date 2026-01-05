using UnityEngine;
using System.Collections.Generic;

public class GlobeCountriesBehaviour : MonoBehaviour
{
    GameObject[] countries;
    public Transform globeSurfaceTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Assign all children to the countries array
        int childCount = transform.childCount;
        countries = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            countries[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < countries.Length; i++)
        {
            CountryBehaviour countryBehaviour = countries[i].GetComponent<CountryBehaviour>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = globeSurfaceTransform.rotation;
    }

    public void ChangeHeights(Dictionary<string, float> data)
    {
        float maxValue = 0f;
        foreach (float value in data.Values)
        {
            if (value > maxValue)
            {
                maxValue = value;
            }
        }

        for (int i = 0; i < countries.Length; i++)
        {
            CountryBehaviour countryBehaviour = countries[i].GetComponent<CountryBehaviour>();
            string countryCode = countryBehaviour.GetIsoA3Code();
            if (data.ContainsKey(countryCode))
            {
                float height = data[countryCode] / maxValue;
                countryBehaviour.SetHeight(height);
            } else
            {
                countryBehaviour.SetHeight(0f);
            }
        }
    }
}
