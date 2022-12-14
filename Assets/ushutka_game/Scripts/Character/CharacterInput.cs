using UnityEngine;

public class CharacterInput : CharacterComponent
{
    Vector2 target;
    public Transform follow;

    float IdleTime
    {
        get => Random.Range(0.25f, 1.25f);
    }

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
        if(follow)
        {
            target = (Vector2)follow.position;
            CharacterEntity.Controller.SetTargetPosition(target);

            return;
        }

        if(!AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser))
        {
            if ((Vector2)transform.position == target)
            {
                (Vector2 _target, Quaternion _) = transform.GetPositionAndRotaion();
                target = _target;

                Invoke(nameof(FindNewTarget), IdleTime);
            }

            return;
        }

        if (Input.GetMouseButton(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!target.CanMove())
            {
                return;
            }

            CharacterEntity.Controller.SetTargetPosition(target);
        }
    }

    float GetIdleTime()
    {
        return Random.Range(0.15f, 1.25f);
    }

    void FindNewTarget()
    {
        CharacterEntity.Controller.SetTargetPosition(target);
    }

    public void SetFollow(Transform _follow)
    {
        follow = _follow;
        transform.SetParent(_follow);
    }
}
