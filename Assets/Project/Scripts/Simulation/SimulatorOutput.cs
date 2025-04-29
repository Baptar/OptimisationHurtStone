using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public abstract class SimulatorOutput
{
    [System.Serializable]
    protected struct SerializableResults
    {
        public List<Result> results;
    }

    public static float GetWinRate(int winnerId)
    {
        ReadOnlyCollection<Result> results = Simulator.GetResults();
        int winCount = results.Select((Result r) => r.winnerId).Where((int id) => id == winnerId).Count();
        return winCount / (float)results.Count;
    }

    public static bool Save(out string filepath)
    {
        string json = JsonUtility.ToJson(new SerializableResults { results = Simulator.GetResults().ToList() });
        filepath = System.DateTime.Now.ToString() + "_" + System.Guid.NewGuid() + ".save";
        return global::Save.SaveJson(json, global::Save.GetPath(filepath), overwrite: false);
    }
}
