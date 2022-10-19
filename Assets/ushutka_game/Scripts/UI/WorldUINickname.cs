using UnityEngine;
using TMPro;

public class WorldUINickname : MonoBehaviour
{
    TextMeshPro nameText;

    private void Start()
    {
        nameText = GetComponent<TextMeshPro>();
    }

    public void UpdateUIName(string nameString)
    {
        nameText.text = nameString;
    }
}
