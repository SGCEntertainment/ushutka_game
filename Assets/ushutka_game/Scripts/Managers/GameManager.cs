using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    List<Bot> bots = new List<Bot>();

    const float maxX = 100.0f;
    const float maxY = 100.0f;

    [SerializeField] Bot botPrefab;
    [SerializeField] Transform botParent;

    private void Start()
    {
        NickUtil.Init();
        AddBotsInGame();
    }

    void AddBotsInGame()
    {
        for (int i = 0; i < 25; i++)
        {
            AddBot();
        }
    }

    public void AddBot()
    {
        (Vector2 position, Quaternion _) = transform.GetPositionAndRotaion();

        Bot bot = Instantiate(botPrefab, position, Quaternion.Euler(Vector3.zero), botParent);
        bot.userInfo = new UserInfo { name = NickUtil.GetName(), level = 0 };

        bots.Add(bot);
    }

    public void RemoveBot(Bot bot)
    {
        bots.Remove(bot);
        Destroy(bot.gameObject);
    }
}
