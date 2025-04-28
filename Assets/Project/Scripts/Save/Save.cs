using System.IO;

public static class Save
{
    public static string GetBasePath()
    {
        return Path.Combine(UnityEngine.Application.dataPath, "Saves");
    }

    public static string GetPath(string filename)
    {
        return Path.Combine(GetBasePath(), filename);
    }

    public static bool Deck(Deck deck, string filename = "deck.json")
    {
        if (deck == null) return false;
        return SaveJson(deck.ToJson(), GetPath(filename));
    }
    

    public static bool SaveJson(string json, string filepath, bool overwrite = true)
    {
        string fullpath = Save.InternalCheck(filepath, overwrite);
        File.WriteAllText(fullpath, json);
        return true;
    }


    private static string InternalCheck(string filepath, bool overwrite)
    {
        // Check not empty file path & overwrite settings
        string fullPath = Path.GetFullPath(filepath);
        if (string.IsNullOrEmpty(fullPath)) throw new System.ArgumentException($"File path cannot be resolved or is empty : {fullPath}");
        if (!overwrite && File.Exists(fullPath)) throw new System.ArgumentException($"File already exists & Overwrite is set to false : {fullPath}");

        // Check file directory & create it if needed
        string dirpath = Path.GetDirectoryName(fullPath);
        if (string.IsNullOrEmpty(dirpath)) throw new System.ArgumentException($"Filepath is invalid : {fullPath}");
        if (!Directory.Exists(dirpath)) Directory.CreateDirectory(dirpath);

        // Return result full file path
        return fullPath;
    }
}
