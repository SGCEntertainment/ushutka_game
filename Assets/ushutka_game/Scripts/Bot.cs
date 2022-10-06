using UnityEngine;
using TMPro;

public class Bot : MonoBehaviour
{
    Vector2 target;

    const float rotSpeed = 560.0f;
    float speed = 3.0f;

    int runID;

    [SerializeField] TextMeshPro statsText;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator anim;

    public UserInfo userInfo;

    private void Start()
    {
        runID = Animator.StringToHash("speed");

        UpdateTarget();
        UpdateStatsText();
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
}
