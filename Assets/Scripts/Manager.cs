using UnityEngine;

public class Manager : MonoBehaviour
{
    ChartBehaviour chartBehaviour;

    CountryBehaviour selectedCountry = null;
    int selectedCountryId = -1;

    void Start()
    {
        chartBehaviour = FindObjectOfType<ChartBehaviour>();
    }

    public void OnCountryClicked(GameObject countryObj)
    {
        FindObjectOfType<Logger>().Log("country_select", gameObject.name);

        if (selectedCountry != null)
        {
            selectedCountry.Deselect();
        }
        CountryBehaviour newCountryBehaviour = countryObj.GetComponent<CountryBehaviour>();
        int newCountryId = newCountryBehaviour.GetCountryID();
        if (newCountryId == selectedCountryId)
        {
            // Deselecting a country
            selectedCountry = null;
            selectedCountryId = -1;
        } else
        {
            // Selecting a new country
            selectedCountry = newCountryBehaviour;
            selectedCountryId = newCountryId;
            selectedCountry.Select();
            UpdateInfoPanel();
        }
    }

    private void UpdateInfoPanel()
    {
        float[] data = DataProvider.GetChartData(selectedCountryId);
        for (int i = 0; i < data.Length; i++)
        {
            data[i] /= 100f;
        }
        chartBehaviour.SetData(data);
    }
}
