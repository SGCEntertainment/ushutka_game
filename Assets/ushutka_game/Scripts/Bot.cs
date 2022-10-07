using UnityEngine;
using System.Collections;
using TMPro;

public class Bot : MonoBehaviour
{
    Vector2 target;

    const float rotSpeed = 560.0f;
    float speed = 3.0f;

    int currentProgress;
    const int count = 4;

    int runID;

    [SerializeField] TextMeshPro statsText;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator anim;

    public UserInfo userInfo;

    IEnumerator Start()
    {
        runID = Animator.StringToHash("speed");
        UpdateTarget();
        UpdateStatsText();

        Vector3 initScale = Vector3.zero;
        Vector3 targetScale = new Vector3(userInfo.level + 1, userInfo.level + 1, 1);

        float et = 0.0f;
        float growingTime = 1.0f;

        while(et < growingTime)
        {
            float t = et / growingTime;
            transform.localScale = Vector3.Lerp(initScale, targetScale, t);

            et += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private void Update()
    {
        Move();
    }

    void UpdateTarget()
    {
        (Vector2 position, Quaternion _) = transform.GetPositionAndRotaion();
        target = position;
    }

    void Move()
    {
        rigid.position = Vector2.MoveTowards(rigid.position, target, speed * Time.deltaTime);

        Vector2 direction = target - (Vector2)transform.position;
        if (direction != Vector2.zero)
        {
            Quaternion targetRotarion = Quaternion.LookRotation(transform.GetChild(0).forward, direction);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, targetRotarion, rotSpeed * Time.deltaTime);
        }

        if ((Vector2)transform.position == target)
        {
            (Vector2 position, Quaternion _) = transform.GetPositionAndRotaion();
            target = position;
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
        if(collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (bot.userInfo.level <= userInfo.level)
        {
            GameManager.Instance.RemoveBot(bot.gameObject);

            currentProgress++;
            if (currentProgress >= count)
            {
                currentProgress = 0;

                userInfo.level++;
                UpdateStatsText();
            }
        }
        else
        {
            userInfo.level--;
            if (userInfo.level <= 0)
            {
                userInfo.level = 0;
                UpdateStatsText();
                GameManager.Instance.RemoveBot(gameObject);
            }
        }

        speed = 3 - transform.localScale.x / 10;
    }
}
