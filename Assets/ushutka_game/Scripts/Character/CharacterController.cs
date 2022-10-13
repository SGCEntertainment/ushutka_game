using UnityEngine;

public class CharacterController : CharacterComponent
{
    Vector2 target;
    Vector2 direction;

    bool IsIdle
    {
        get => (Vector2)transform.position == target;
    }

    Rigidbody2D rigid;

    [SerializeField] float rotSpeed;
    [SerializeField] float moveSpeed;

    private void Start()
    {
        target = transform.position;
        rigid = CharacterEntity.Rigidbody;
    }

    private void Update()
    {
        UpdateRotation();
        UpdatePosition();
        UpdateAnimation();
    }

    void UpdateRotation()
    {
        direction = target - rigid.position;
        if (direction != Vector2.zero)
        {
            Quaternion targetRotarion = Quaternion.LookRotation(transform.GetChild(0).forward, direction);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, targetRotarion, rotSpeed * Time.deltaTime);
        }
    }

    void UpdateAnimation()
    {
        CharacterEntity.Animator.SetSpeed(IsIdle);
    }

    void UpdatePosition()
    {
        transform.position = Vector2.MoveTowards(rigid.position, target, moveSpeed * Time.deltaTime);
    }

    public void SetTargetPosition(Vector2 position)
    {
        target = position;
    }
}
