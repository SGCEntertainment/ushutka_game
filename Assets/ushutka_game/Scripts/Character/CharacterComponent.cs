using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public CharacterEntity CharacterEntity { get;private set; }

    public virtual void Init(CharacterEntity _characterEntity)
    {
        CharacterEntity = _characterEntity;
    }

    public virtual void OnBattleStart() { }

    public virtual void OnBattleEnd() { }
}
