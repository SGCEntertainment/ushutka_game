using UnityEngine;

public static class LanguageUtil
{
    const string ruLangCode = "RU";
    const string enLangCode = "EN";

    const string language_key = "currentlanguage";

    public static bool LanguageIsSet()
    {
        return PlayerPrefs.HasKey(language_key);
    }

    public static bool IsRussian()
    {
        return LanguageIsSet() && PlayerPrefs.GetString(language_key) == ruLangCode;
    }

    public static void SetLanguage(bool setRussian)
    {
        PlayerPrefs.SetString(language_key, setRussian? ruLangCode : enLangCode);
        PlayerPrefs.Save();
    }
}
