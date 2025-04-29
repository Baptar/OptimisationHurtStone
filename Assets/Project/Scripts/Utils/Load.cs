using System.IO;

public static class Load
{
    public static string GetBasePath()
    {
        return Save.GetBasePath();
    }

    public static string GetPath(string filename)
    {
        return Save.GetPath(filename);
    }

    public static bool Deck(out Deck deck, string filename = "deck.json")
    {
        bool success = LoadJson(out string json, GetPath(filename));
        if (success) return global::Deck.FromJson(json, out deck);
        deck = default;
        return false;
    }


    public static bool LoadJson(out string json, string filepath, bool throwException = false)
    {
        // Check internal validations & exceptions
        string fullpath = Load.InternalCheck(filepath);
        if (!Load.InternalExceptionCheck(filepath, throwException)) {
            json = string.Empty;
            return false;
        }

        // Read result & ouput it
        json = File.ReadAllText(fullpath);
        return true;
    }


    private static string InternalCheck(string filepath)
    {
        // Check not empty file path & overwrite settings
        string fullPath = Path.GetFullPath(filepath);
        if (string.IsNullOrEmpty(fullPath)) throw new System.ArgumentException($"File path cannot be resolved or is empty : {fullPath}");
        return fullPath; // Return result full file path
    }

    private static bool InternalExceptionCheck(string filepath, bool throwException)
    {
        if (!File.Exists(filepath)) {
            if (throwException) throw new System.ArgumentException($"File does not exists : {filepath}");
            return false;
        }
        return true;
    }
}
