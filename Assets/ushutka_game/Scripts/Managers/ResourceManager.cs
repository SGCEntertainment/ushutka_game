using UnityEngine;
using System.Collections.Generic;

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
            }

            return instance;
        }
    }

    public WorldUINickname worldUINickname;
    public List<CharacterDefinition> characterDefinitions;
}