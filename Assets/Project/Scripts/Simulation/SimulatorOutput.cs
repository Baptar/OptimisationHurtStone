using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SimulatorOutput
{
    [System.Serializable]
    protected struct SerializableResults
    {
        public List<Result> results;
    }

    public static bool Save(out string filepath)
    {
        string json = JsonUtility.ToJson(new SerializableResults { results = Simulator.GetResults().ToList() });
        filepath = System.DateTime.Now.ToString() + "_" + System.Guid.NewGuid() + ".save";
        return global::Save.SaveJson(json, global::Save.GetPath(filepath), overwrite: false);
    }
}
