using UnityEngine;

public static class UserInfo
{
    public static string Username
    {
        get => PlayerPrefs.GetString("Username", string.Empty);
        set => PlayerPrefs.SetString("Username", value);
    }

    public static int SkinID
    {
        get => PlayerPrefs.GetInt("SkinID", 0);
        set => PlayerPrefs.SetInt("SkinID", value);
    }
}
