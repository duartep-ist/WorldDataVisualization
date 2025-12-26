using UnityEngine;

public class GlobeCountriesBehaviour : MonoBehaviour
{
    GameObject[] countries;
    public Transform globeSurfaceTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Assign all children to the countries array
        int childCount = transform.childCount;
        countries = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            countries[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = globeSurfaceTransform.rotation;
    }
}
