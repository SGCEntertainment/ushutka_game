using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 target;

    const float rotSpeed = 560.0f;
    float speed = 3.0f;

    Vector2 direction;

    int runID;

    [SerializeField] TextMeshPro statsText;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator anim;

    public UserInfo userInfo;

    private void Start()
    {
        runID = Animator.StringToHash("speed");
        UpdateStatsText();
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
}
