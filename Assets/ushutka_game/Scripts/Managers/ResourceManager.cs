using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<ResourceManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    //public GameUI hudPrefab;
    //public NicknameUI nicknameCanvasPrefab;
    public CharacterDefinition[] characterDefinitions;
}
