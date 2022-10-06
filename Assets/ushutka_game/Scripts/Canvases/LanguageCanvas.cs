using UnityEngine;

public class LanguageCanvas : MonoBehaviour
{
    public void SetLanguage(bool setRussian)
    {
        LanguageUtil.SetLanguage(setRussian);
        UIManager.Instance.ShowCanvas(CanvasName.splash);

        Destroy(gameObject);
    }
}
