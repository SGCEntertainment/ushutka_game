using UnityEngine;

public static class NickUtil
{
    static TextAsset namesFile;
    static string[] names;

    public static void Init()
    {
        namesFile = (TextAsset)Resources.Load("names");
        names = namesFile.text.Split('\n');
    }

    public static string GetName()
    {
        return names[Random.Range(0, names.Length)].Replace("\r", "");
    }
}
