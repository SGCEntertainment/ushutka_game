using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldUINickname : MonoBehaviour
{
    CharacterEntity _characterEntity;

    [SerializeField] Vector3 offset;
    [SerializeField] Text worldNicknameText;

    [HideInInspector] public Transform target;

    private void LateUpdate()
    {
        if (target)
        {
            transform.position = target.position + offset;
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            StartCoroutine(WaitAndDestroy());
        }
    }

    private void Update()
    {
        if (_characterEntity == null)
        {
            return;
        }

        var lobbyUser = _characterEntity.RoomUser;
        if (lobbyUser != null)
        {
            worldNicknameText.text = lobbyUser.Username;
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3);
        if (target != null && !target.Equals(null))
        {
            //continue following the target
            yield return null;
        }
        else //there has been no target to follow for 3 seconds so Destroy this:
        {
            Destroy(gameObject);
        }
    }
}
