public static class AuthorityUtil
{
    public static bool HasInputAuthority(RoomPlayer roomPlayer)
    {
        return roomPlayer.spawnedId == 0;
    }
}
