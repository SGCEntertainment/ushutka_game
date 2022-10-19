using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayer : CharacterComponent
{
    public int spawnedId;
    public CharacterEntity prefab;

    public static RoomPlayer Local;

    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;

    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

    public CharacterController Character { get; set; }

    public WorldUINickname WorldUINickname;

    public string Username { get; set; }

    public int SkinID { get; set; }

    private void Start()
    {
        Username = NickUtil.GetName();
        //if (AuthorityUtil.HasInputAuthority(this))
        //{
        //    Local = this;

        //    PlayerChanged?.Invoke(this);
        //    SetPlayerStats(UserInfo.Username, UserInfo.SkinID);
        //}
        //else
        //{
        //    Username = NickUtil.GetName();
        //}

        Players.Add(this);
        PlayerJoined?.Invoke(this);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        WorldUINickname.UpdateUIName($"{Username}({CharacterEntity.ProgressController.level})");
    }

    private void SetPlayerStats(string username, int skinID)
    {
        Username = username;
        SkinID = skinID;
    }
}
