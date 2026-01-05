using System.Collections;
using System.Collections.Generic;

public struct ChartEntry
{
    public string label;
    public float value;

    public ChartEntry(string label, float value)
    {
        this.label = label;
        this.value = value;
    }
}


public class DataProvider
{
    public static string GetCountryName(string countryCode)
    {
        // FIXME
        return "Country " + countryCode;
    }

    public static ChartEntry[] GetChartData(string countryCode)
    {
        // FIXME
        List<ChartEntry> entries = new List<ChartEntry>();
        entries.Add(new ChartEntry("2018", 100.0f));
        entries.Add(new ChartEntry("2019", 200.0f));
        entries.Add(new ChartEntry("2020", 300.0f));
        entries.Add(new ChartEntry("2021", 400.0f));
        entries.Add(new ChartEntry("2022", 500.0f));
        return entries.ToArray();
    }

    public static Dictionary<string, float> GetGlobeData()
    {
        // FIXME
        Dictionary<string, float> data = new Dictionary<string, float>();
        data["USA"] = 50.0f;
        data["CAN"] = 30.0f;
        data["MEX"] = 20.0f;
        data["POR"] = 20.0f;
        data["ESP"] = 20.0f;
        return data;
    }
}
