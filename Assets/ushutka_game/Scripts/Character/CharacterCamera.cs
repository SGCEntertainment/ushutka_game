using UnityEngine;

public class CharacterCamera : CharacterComponent
{
    private void LateUpdate()
    {
        if (AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser))
        {
            ControlCamera(Camera.main);
        }
    }

    private void ControlCamera(Camera cam)
    {
        cam.transform.position = transform.position;
    }
}
