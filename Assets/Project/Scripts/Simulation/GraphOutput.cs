using System.Collections.ObjectModel;
using XCharts.Runtime;

public class GraphOutput : SimulatorOutput
{
    public LineChart lineChart;

    public void Apply()
    {
        ReadOnlyCollection<Result> results = Simulator.GetResults();
        lineChart.ClearData();


        // Set graph values (with x & y values)
        //const int graphStepY = 30;
        for (int i = 0; i <= /*results.Count*/ 200; i += 1) {
            lineChart.AddData($"serie0", i, i);
        }

        // Set Legend on axis X (Here 1 anotation every 'graphStep' results)
        const int graphStepX = 50;
        for (int i = graphStepX; i <= /*results.Count*/ 400; i += graphStepX) {
            lineChart.AddXAxisData("t:" + i.ToString());
        }

        // Apply Y axis Legend & Inverval spacing
        YAxis axis = lineChart.GetChartComponent<YAxis>();
        axis.interval = 25.0f;
    }
}