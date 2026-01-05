using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    ChartBehaviour chartBehaviour;
    GlobeCountriesBehaviour globeCountriesBehaviour;
    CountryBehaviour selectedCountry = null;
    string selectedCountryCode = "";

    public TextMeshProUGUI countryNameText;

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
        for (int i = 0; i < data.Length; i++)
        {
            data[i].value /= 100f;
        }
        chartBehaviour.SetData(data);
    }
}
