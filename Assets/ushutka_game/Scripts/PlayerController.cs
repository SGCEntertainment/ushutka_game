using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Vector2 target;

    const float rotSpeed = 560.0f;
    [SerializeField] float speed = 3.0f;

    Vector2 direction;

    int runID;

    int currentProgress;
    const int count = 4;

    Vector3 initScale;
    Vector3 targetScale;

    [SerializeField] TextMeshPro statsText;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator anim;

    public UserInfo userInfo;

    void Start()
    {
        currentProgress = 0;
        runID = Animator.StringToHash("speed");
        UpdateStatsText();

        initScale = Vector3.zero;
        targetScale = new Vector3(userInfo.level + 1, userInfo.level + 1, 1);

        StartCoroutine(nameof(Growing));
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - rigid.position;

            target = mousePos;
        }

        Move();
    }

    void Move()
    {
        rigid.position = Vector2.MoveTowards(rigid.position, target, speed * Time.deltaTime);

        direction = target - (Vector2)transform.position;
        if (direction != Vector2.zero)
        {
            Quaternion targetRotarion = Quaternion.LookRotation(transform.GetChild(0).forward, direction);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, targetRotarion, rotSpeed * Time.deltaTime);
        }

        anim.SetFloat(runID, Mathf.Clamp(direction.sqrMagnitude, 0, speed));
    }

    public void UpdateStatsText()
    {
        statsText.text = $"{userInfo.name}({userInfo.level + 1})";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bot bot = collision.gameObject.GetComponent<Bot>();
        if(bot.userInfo.level <= userInfo.level)
        {
            GameManager.Instance.RemoveBot(bot.gameObject);

            currentProgress++;
            if(currentProgress>= count)
            {
                currentProgress = 0;

                userInfo.level++;
                UpdateStatsText();

                GameManager.maxLevelInGame = userInfo.level + Random.Range(2, 5);

                initScale = transform.localScale;
                targetScale = new Vector3(userInfo.level + 1, userInfo.level + 1, 1);
                StartCoroutine(nameof(Growing));
            }
        }
        else
        {
            userInfo.level--;
            initScale = transform.localScale;
            targetScale = new Vector3(userInfo.level + 1, userInfo.level + 1, 1);

            if (userInfo.level <= 0)
            {
                userInfo.level = 0;
                UpdateStatsText();
                //game over
            }

            StartCoroutine(nameof(Growing));
        }
    }

    IEnumerator Growing()
    {
        float et = 0.0f;
        float growingTime = 1.0f;

        while (et < growingTime)
        {
            float t = et / growingTime;
            transform.localScale = Vector3.Lerp(initScale, targetScale, t);

            et += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        initScale = targetScale;

        speed = 3 - transform.localScale.x / 10;
    }
}
