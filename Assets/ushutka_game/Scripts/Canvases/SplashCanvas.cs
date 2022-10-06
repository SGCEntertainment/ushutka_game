using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class SplashCanvas : MonoBehaviour
{
    [SerializeField] float duration;

    [Space(10)]
    [SerializeField] Image ball;
    [SerializeField] Image text;

    [Space(10)]
    [SerializeField] Sprite ruSprite;
    [SerializeField] Sprite enSprite;

    private void Start()
    {
        SetTextSprite();
        StartCoroutine(nameof(Init));
    }

    void SetTextSprite()
    {
        text.sprite = LanguageUtil.IsRussian() ? ruSprite : enSprite;
        text.SetNativeSize();
    }

    IEnumerator Init()
    {
        text.color = new Color(1, 1, 1, 0);

        Vector2 ballInitPos = Vector2.down * Screen.height / 2 + Vector2.down * ball.rectTransform.sizeDelta.y / 2;
        Vector2 ballTragetPos = new Vector2(0, 80);
        ball.transform.localPosition = ballInitPos;

        float et = 0.0f;
        float ballTime = 2 * duration / 3;
        float textTime = duration / 3;

        while (et < ballTime)
        {
            et += Time.deltaTime;

            float t = et / ballTime;
            ball.transform.localPosition = Vector2.Lerp(ballInitPos, ballTragetPos, t);

            yield return null;
        }

        et = 0.0f;
        while (et < textTime)
        {
            et += Time.deltaTime;

            float t = et / textTime;
            text.color = new Color(1, 1, 1, t);

            yield return null;
        }

        UIManager.Instance.ShowCanvas(CanvasName.menu);
        Destroy(gameObject);
    }
}
