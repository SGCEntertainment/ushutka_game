using UnityEngine;
using UnityEngine.UI;

public class ProgressCanvas : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Sprite[] achievementSprites;

    private void Start()
    {
        CheckProgress();
    }

    void CheckProgress()
    {
        Image[] icons = new Image[parent.childCount];
        for(int i = 0; i < icons.Length; i++)
        {
            icons[i] = parent.GetChild(i).GetChild(1).GetComponent<Image>();
            icons[i].gameObject.SetActive(false);
        }
    }
}
