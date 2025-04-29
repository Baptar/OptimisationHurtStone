using UnityEngine;
using XCharts.Runtime;

public class SimTest : MonoBehaviour
{
    [SerializeField]
    private LineChart lineChart;
    private GraphOutput output = new GraphOutput();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        output.lineChart = lineChart;
        output.Apply();
    }
}
