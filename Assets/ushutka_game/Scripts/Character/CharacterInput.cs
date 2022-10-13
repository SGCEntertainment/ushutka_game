using UnityEngine;

public class CharacterInput : CharacterComponent
{
    private void Update()
    {
        if(!AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser))
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CharacterEntity.Controller.SetTargetPosition(mousePos);
        }
    }
}
