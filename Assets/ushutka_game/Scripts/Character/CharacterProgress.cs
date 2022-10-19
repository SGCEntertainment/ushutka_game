using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CharacterProgress : CharacterComponent
{
    public bool im;

    public int level;
    public int count;

    public CharacterEntity followerRef;

    Transform[] places;

    public List<CharacterEntity> characterEntities = new List<CharacterEntity>();

    private void Start()
    {
        im = AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser);
        level = AuthorityUtil.HasInputAuthority(CharacterEntity.RoomUser) ? 1 : Random.Range(1,1);

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
        CharacterProgress collided = collision.gameObject.GetComponent<CharacterProgress>();

        if(collided.CharacterEntity.ProgressController.followerRef == CharacterEntity)
        {
            return;
        }

        if(level > collided.level || (level == collided.level && im) || level == collided.level && count > collided.count)
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
            collided.CharacterEntity.ProgressController.SetParentRef(followerRef ? followerRef : CharacterEntity);
            collided.CharacterEntity.ProgressController.level = followerRef ? followerRef.ProgressController.level : level;
            collided.CharacterEntity.Controller.moveSpeed = CharacterEntity.Controller.moveSpeed;
            collided.CharacterEntity.ProgressController.im = im;

            if (collided.CharacterEntity.RoomUser.WorldUINickname)
            {
                Destroy(collided.CharacterEntity.RoomUser.WorldUINickname.gameObject);
            }

            characterEntities.Add(collided.CharacterEntity);
            if(followerRef)
            {
                followerRef.CharacterEntity.ProgressController.count++;
            }
            else
            {
                count++;
            }
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
