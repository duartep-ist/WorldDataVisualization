using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    ChartBehaviour chartBehaviour;
    GlobeCountriesBehaviour globeCountriesBehaviour;
    CountryBehaviour selectedCountry = null;
    string selectedCountryCode = "";

    public TextMeshProUGUI countryNameText;
    public TextMeshProUGUI chartMaxText;

    void Start()
    {
        chartBehaviour = FindObjectOfType<ChartBehaviour>();
        globeCountriesBehaviour = FindObjectOfType<GlobeCountriesBehaviour>();

        globeCountriesBehaviour.ChangeHeights(DataProvider.GetGlobeData());
    }

    public void OnCountryClicked(GameObject countryObj)
    {
        FindObjectOfType<Logger>().Log("country_select", gameObject.name);

        if (selectedCountry != null)
        {
            selectedCountry.Deselect();
        }
        CountryBehaviour newCountryBehaviour = countryObj.GetComponent<CountryBehaviour>();
        string newCountryCode = newCountryBehaviour.GetIsoA3Code();
        if (newCountryCode == selectedCountryCode)
        {
            // Deselecting a country
            selectedCountry = null;
            selectedCountryCode = "";
        } else
        {
            // Selecting a new country
            selectedCountry = newCountryBehaviour;
            selectedCountryCode = newCountryCode;
            selectedCountry.Select();
            UpdateInfoPanel();
        }
    }

    private void UpdateInfoPanel()
    {
        countryNameText.text = DataProvider.GetCountryName(selectedCountryCode);

        ChartEntry[] data = DataProvider.GetChartData(selectedCountryCode);

        // Find the maximum value to normalize heights
        float maxValue = 0f;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].value > maxValue)
            {
                maxValue = data[i].value;
            }
        }

        chartMaxText.text = maxValue.ToString("F0");

        float [] normalizedData = new float[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            normalizedData[i] = data[i].value / maxValue;
        }
        chartBehaviour.SetData(normalizedData);
    }
}
