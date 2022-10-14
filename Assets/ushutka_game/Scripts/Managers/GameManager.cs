using System.Resources;
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
    const int playersInRoom = 25;

    float targetCamSize;
    const float maxDelta = 0.25f;

    public static int maxLevelInGame;

    //[SerializeField] Bot botPrefab;
    [SerializeField] Transform parent;

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

        for(int i = 0; i < playersInRoom; i++)
        {
            SpawnPlayer(i);
        }
    }

    public void SpawnPlayer(int spawnedId)
    {
        (Vector2 position, Quaternion rotation) = transform.GetPositionAndRotaion();

        var prefabId = Random.Range(0, ResourceManager.Instance.characterDefinitions.Count);
        var prefab = ResourceManager.Instance.characterDefinitions[prefabId].prefab;

        var entity = Instantiate(prefab, position, rotation, parent);

        entity.RoomUser.Username = NickUtil.GetName();
        entity.RoomUser.spawnedId = spawnedId;

        Debug.Log($"Spawning character for {entity.RoomUser.Username} as {prefab.name}");
        entity.transform.name = $"{prefab.name} ({entity.RoomUser.Username})";
    }
}
