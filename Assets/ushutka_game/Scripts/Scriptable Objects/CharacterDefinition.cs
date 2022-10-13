using UnityEngine;

[CreateAssetMenu(fileName = "New Character Definition", menuName = "Scriptable Object/Character Definition")]
public class CharacterDefinition : ScriptableObject
{
    public CharacterEntity prefab;
}
