using System;
using UnityEngine;

public class CharacterEntity : CharacterComponent
{
    public static event Action<CharacterEntity> OnCharacterSpawned;
    public static event Action<CharacterEntity> OnCharacterDespawned;

    public CharacterAnimator Animator { get; private set; }
    public CharacterCamera Camera { get; private set; }
    public CharacterController Controller { get; private set; }
    public CharacterInput Input { get; private set; }
    public CharacterProgressController ProgressController { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public RoomPlayer RoomUser { get; set; }

    private void Awake()
    {
        // Set references before initializing all components
        Animator = GetComponentInChildren<CharacterAnimator>();
        Camera = GetComponent<CharacterCamera>();
        Controller = GetComponent<CharacterController>();
        Input = GetComponent<CharacterInput>();
        ProgressController = GetComponent<CharacterProgressController>();
        Rigidbody = transform.GetChild(0).GetComponent<Rigidbody2D>();
        RoomUser = GetComponent<RoomPlayer>();

        // Initializes all KartComponents on or under the Kart prefab
        var components = GetComponentsInChildren<CharacterComponent>();
        foreach (var component in components)
        {
            component.Init(this);
        }
    }
}
