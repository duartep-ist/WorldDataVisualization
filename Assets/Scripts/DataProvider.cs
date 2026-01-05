using System.Collections.Generic;
using UnityEngine;

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

public struct MetricEntry
{
    public int year;
    public float value;

    public MetricEntry(int year, float value)
    {
        this.year = year;
        this.value = value;
    }
}


public class DataProvider
{
    private const string WORLD_DATA_PATH = "Data/world_data_2018_2022";
    private const string AVG_GDP_PATH = "Data/avg_gdp_per_country";

    private static bool loaded = false;

    private static List<Row> rows = new List<Row>();
    private static Dictionary<string, float> avgGdpByCountry = new Dictionary<string, float>();
    private static Dictionary<string, string> countryNames = new Dictionary<string, string>();

    // Internal data representation
    private struct Row
    {
        public string entity;
        public string code;
        public int year;
        public float gdp;
        public float unemployment;
        public float co2;
        public float lifeExpectancy;
    }

    // ----------------- PUBLIC API -----------------

    // Average GDP per country
    public static Dictionary<string, float> GetGlobeData()
    {
        LoadIfNeeded();
        return new Dictionary<string, float>(avgGdpByCountry);
    }

    // Country names
    public static string GetCountryName(string countryCode)
    {
        LoadIfNeeded();
        return countryNames.ContainsKey(countryCode) ? countryNames[countryCode] : countryCode;
    }

    public static ChartEntry[] GetChartData(string countryCode)
    {
        LoadIfNeeded();

        List<ChartEntry> entries = new List<ChartEntry>();
        foreach (var r in rows)
        {
            if (r.code == countryCode)
                entries.Add(new ChartEntry(r.year.ToString(), r.gdp));
        }
        return entries.ToArray();
    }

    // Other metrics: use MetricEntry
    public static MetricEntry[] GetUnemploymentRate(string countryCode)
    {
        return GetMetricSeries(countryCode, r => r.unemployment);
    }

    public static MetricEntry[] GetCO2PerCapita(string countryCode)
    {
        return GetMetricSeries(countryCode, r => r.co2);
    }

    public static MetricEntry[] GetLifeExpectancy(string countryCode)
    {
        return GetMetricSeries(countryCode, r => r.lifeExpectancy);
    }

    // ----------------- INTERNAL HELPER -----------------
    private static MetricEntry[] GetMetricSeries(string countryCode, System.Func<Row, float> selector)
    {
        LoadIfNeeded();

        List<MetricEntry> entries = new List<MetricEntry>();
        foreach (var r in rows)
        {
            if (r.code == countryCode)
                entries.Add(new MetricEntry(r.year, selector(r)));
        }
        return entries.ToArray();
    }

    private static void LoadIfNeeded()
    {
        if (loaded) return;

        LoadWorldData();
        LoadAverageGDP();

        loaded = true;
    }

    private static void LoadWorldData()
    {
        TextAsset csv = Resources.Load<TextAsset>(WORLD_DATA_PATH);
        string[] lines = csv.text.Split('\n');

        // Skip header
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cols = lines[i].Split(',');

            Row r = new Row
            {
                entity = cols[0],
                code = cols[1],
                year = int.Parse(cols[2]),
                gdp = ParseFloat(cols[3]),
                unemployment = ParseFloat(cols[4]),
                co2 = ParseFloat(cols[5]),
                lifeExpectancy = ParseFloat(cols[6])
            };

            rows.Add(r);

            if (!countryNames.ContainsKey(r.code))
                countryNames[r.code] = r.entity;
        }
    }

    private static void LoadAverageGDP()
    {
        TextAsset csv = Resources.Load<TextAsset>(AVG_GDP_PATH);
        string[] lines = csv.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cols = lines[i].Split(',');

            string code = cols[1];
            float avgGdp = ParseFloat(cols[2]);

            avgGdpByCountry[code] = avgGdp;
        }
    }

    private static float ParseFloat(string s)
    {
        float.TryParse(s, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out float v);
        return v;
    }
}
