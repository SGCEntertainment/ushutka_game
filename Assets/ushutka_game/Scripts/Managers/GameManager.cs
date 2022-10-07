using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    int currentBotsCount;
    const int botsInRoom = 15;

    float targetCamSize;
    const float maxDelta = 0.25f;

    public static int maxLevelInGame;

    [SerializeField] Bot botPrefab;
    [SerializeField] Transform botParent;

    [Space(10)]
    [SerializeField] Camera _cam;

    private void Start()
    {
        #if !UNITY_EDITOR

        if(!string.Equals(Application.absoluteURL, "https://hagi-wagi-eater.netlify.app/"))
        {
            return;
        }

        #endif

        NickUtil.Init();
        AddBotsInGame(botsInRoom);

        InvokeRepeating(nameof(CheckBotsCount), 0.0f, 1.0f);
        maxLevelInGame = 1;
    }

    private void LateUpdate()
    {
        targetCamSize = maxLevelInGame + 5;
        _cam.orthographicSize = Mathf.MoveTowards(_cam.orthographicSize, targetCamSize, maxDelta * Time.deltaTime);
    }

    void CheckBotsCount()
    {
        if(currentBotsCount != botsInRoom)
        {
            int _count = botsInRoom - currentBotsCount;
            AddBotsInGame(_count);
        }
    }

    void AddBotsInGame(int count)
    {
        for (int i = 0; i < count; i++)
        {
            currentBotsCount++;
            AddBot();
        }
    }

    public void AddBot()
    {
        (Vector2 position, Quaternion _) = transform.GetPositionAndRotaion();

        Bot bot = Instantiate(botPrefab, position, Quaternion.Euler(Vector3.zero), botParent);
        bot.userInfo = new UserInfo { name = NickUtil.GetName(), level = Random.Range(0, maxLevelInGame) };
    }

    public void RemoveBot(GameObject bot)
    {
        Destroy(bot);

        currentBotsCount--;
        if(currentBotsCount <= 0)
        {
            currentBotsCount = 0;
        }
    }
}
