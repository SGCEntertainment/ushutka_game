using UnityEngine.UI;
using UnityEngine;

public class NavigationCanvas : MonoBehaviour
{
    private static NavigationCanvas instacne;
    public static NavigationCanvas Instance
    {
        get
        {
            if(!instacne)
            {
                instacne = FindObjectOfType<NavigationCanvas>();
            }

            return instacne;
        }
    }

    GameObject panel;

    [SerializeField] Button backGo;
    [SerializeField] GameObject statsGO;

    [Space(10)]
    [SerializeField] Text headText;

    private void Start()
    {
        panel = transform.GetChild(0).gameObject;
        backGo.onClick.AddListener(UIManager.Instance.Back);
    }

    public void UpdateNavigation(bool canBack = true, string headString = "", bool showStats = false, bool show = false)
    {
        panel.SetActive(show);
        if(!panel.activeSelf)
        {
            return;
        }

        backGo.gameObject.SetActive(canBack);
        statsGO.SetActive(showStats);

        headText.gameObject.SetActive(!string.Equals(headText.text, string.Empty));
        headText.text = headString;
    }
}
