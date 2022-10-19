using UnityEngine;

public class CharacterController : CharacterComponent
{
    Vector2 target;
    Vector2 direction;

    public bool IsIdle
    {
        get => CharacterEntity.ProgressController.followerRef ? 
            CharacterEntity.ProgressController.followerRef.Controller.IsIdle : (Vector2)transform.position == target;
    }

    Rigidbody2D rigid;

    [SerializeField] float rotSpeed;
    public float moveSpeed;

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
