using UnityEngine;
using System.Collections.Generic;

public class CharacterProgress : CharacterComponent
{
    public int level;
    public int count;

    public CharacterEntity followerRef;

    Transform[] places;

    public List<CharacterEntity> characterEntities = new List<CharacterEntity>();

    private void Start()
    {
        level = AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser) ? 1 : 1;

        places = new Transform[transform.parent.GetChild(1).childCount];
        for(int i = 0; i < places.Length; i++)
        {
            places[i] = transform.parent.GetChild(1).GetChild(i);
        }
    }

    Transform GetFreePlace()
    {
        if (places[0].childCount == 0)
        {
            return places[0];
        }
        if (places[1].childCount == 0)
        {
            return places[1];
        }
        else
        {
            return places[2];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool im = AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser);
        CharacterProgress collided = collision.gameObject.GetComponent<CharacterProgress>();

        if(collided.CharacterEntity.ProgressController.followerRef == CharacterEntity)
        {
            return;
        }

        if(level > collided.level || (level == collided.level && im))
        {
            if (count >= 3)
            {
                level++;
                count = 0;

                foreach(CharacterEntity character in characterEntities)
                {
                    Destroy(character.gameObject);
                }

                characterEntities.Clear();
                foreach(Transform t in places)
                {
                    t.DetachChildren();
                }
            }

            Transform freePlace = followerRef ? followerRef.ProgressController.GetFreePlace() : GetFreePlace();
            collided.CharacterEntity.Input.SetFollow(freePlace);
            collided.CharacterEntity.ProgressController.SetParentRef(CharacterEntity);

            Destroy(collided.CharacterEntity.RoomUser.WorldUINickname.gameObject);

            characterEntities.Add(collided.CharacterEntity);
            count++;
        }
    }

    public void SetInitPorgress(int _level, int _count)
    {
        level = _level;
        count = _count;
    }

    public void SetParentRef(CharacterEntity _followerRef)
    {
        followerRef = _followerRef;
    }
}
