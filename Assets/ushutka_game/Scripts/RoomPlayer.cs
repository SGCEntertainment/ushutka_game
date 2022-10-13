using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{
    [HideInInspector]
    public int spawnedId;

    public static RoomPlayer Local;

    public static Action<RoomPlayer> PlayerJoined;
    public static Action<RoomPlayer> PlayerLeft;
    public static Action<RoomPlayer> PlayerChanged;

    public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

    public CharacterController Character { get; set; }

    public string Username { get; set; }

    public int SkinID { get; set; }

    private void Start()
    {
        if (AuthorityUtil.HasInputAuthority(this))
        {
            Local = this;

            PlayerChanged?.Invoke(this);
            SetPlayerStats(UserInfo.Username, UserInfo.SkinID);
        }

        Players.Add(this);
        PlayerJoined?.Invoke(this);

        DontDestroyOnLoad(gameObject);
    }

    private void SetPlayerStats(string username, int skinID)
    {
        Username = username;
        SkinID = SkinID;
    }
}
