using UnityEngine;

public class SimulatorController : MonoBehaviour
{
    [SerializeField]
    private int simulationCount = 500;
    [SerializeField]
    private bool forceRun = false;

    void Start()
    {
        Simulator.OnEnded -= Simulator_OnEnded;
        Simulator.OnEnded += Simulator_OnEnded;
        Simulator.Run(simulationCount);
    }

    private void Simulator_OnEnded()
    {
        Simulator.OnEnded -= Simulator_OnEnded;
        Debug.Log("Winrate : " + SimulatorOutput.GetWinRate(winnerId: 0));
    }

    void Update()
    {
        if (!forceRun) return;
        forceRun = false;
        Start();
    }
}
