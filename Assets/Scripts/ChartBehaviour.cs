using UnityEngine;

public class ChartBehaviour : MonoBehaviour
{
    GameObject[] bars;
    void Start()
    {
        // Assign all children to bars array
        int childCount = transform.childCount;
        bars = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            bars[i] = transform.GetChild(i).gameObject;
        }
    }

    // `data` is an array of float values between 0 and 1 representing the heights of the bars.
    public void SetData(float[] data)
    {
        int dataCount = data.Length;
        for (int i = 0; i < bars.Length; i++)
        {
            float height = (i < dataCount) ? data[i] : 0f;
            Vector3 scale = bars[i].transform.localScale;
            scale.y = height;
            bars[i].transform.localScale = scale;
        }
    }
}
