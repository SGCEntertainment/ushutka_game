using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using UnityEditor.Animations;
using System.Threading;

public class CharacterCreatorLogHandler : ILogHandler
{
    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        Debug.unityLogger.logHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        Debug.unityLogger.LogException(exception, context);
    }
}

public static class CharacterCreatorUtil
{
    static Logger myLogger = new Logger(new CharacterCreatorLogHandler());

    const string IFN = "IDLE";
    const string RFN = "RUN";

    const string rootName = "ushutka_game";

    readonly static string destpath = Path.Combine(Application.dataPath, rootName, "Sprites", "characters");

    const string tag = "Character Creator";

    [MenuItem("Character Creator/Create new Character")]
    static void MakeNewCharacter()
    {
        string sourcepath = EditorUtility.OpenFolderPanel("Create new Character", "", "");
        if (sourcepath == null || sourcepath.Length == 0)
        {
            myLogger.Log(LogType.Error, tag, "Неверный пусть к персонажу");
            return;
        }

        var subDirs = Directory.GetDirectories(sourcepath);
        var folders = subDirs.Select(dir => new DirectoryInfo(dir).Name);

        bool trueFolders = folders.Contains(IFN) && folders.Contains(RFN);
        if (!trueFolders)
        {
            myLogger.Log(LogType.Error, tag, "не удалось найти нужные папки");
        }

        string newCharacterDir = Path.Combine(destpath, new DirectoryInfo(sourcepath).Name);
        Directory.CreateDirectory(newCharacterDir);

        EditorUtility.DisplayProgressBar("Create new Character", "Copying sprites...", 0.2f);
        Thread.Sleep(1000);

        CopyFilesRecursively(sourcepath, newCharacterDir);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();

        string animatorPath = Path.Combine(Application.dataPath, rootName, "Animations", new DirectoryInfo(sourcepath).Name);
        Directory.CreateDirectory(animatorPath);

        EditorUtility.DisplayProgressBar("Create new Character", "Preparing animations...", 0.4f); ;
        Thread.Sleep(1000);

        string animatorFullName = @Path.Combine("Assets", rootName, "Animations", new DirectoryInfo(sourcepath).Name, new DirectoryInfo(sourcepath).Name + ".controller");
        var controller = AnimatorController.CreateAnimatorControllerAtPath(animatorFullName);

        controller.AddParameter("IsIdle", AnimatorControllerParameterType.Bool);
        var rootStateMachine = controller.layers[0].stateMachine;

        var idleState = rootStateMachine.AddState("Idle");
        var runState = rootStateMachine.AddState("Run");

        var toRunTransition = idleState.AddTransition(runState);
        toRunTransition.AddCondition(AnimatorConditionMode.IfNot, 0, "IsIdle");
        toRunTransition.hasExitTime = false;

        var toIdleTransition = runState.AddTransition(idleState);
        toIdleTransition.AddCondition(AnimatorConditionMode.If, 0, "IsIdle");
        toIdleTransition.hasExitTime = false;

        string[] filesInIdle = Directory.GetFiles(Path.Combine(sourcepath, IFN), "*.png");
        AnimationClip idleClip = CreateAnimationClip(filesInIdle, new DirectoryInfo(sourcepath).Name, IFN, "Idle");
        idleState.motion = idleClip;

        string[] filesInRun = Directory.GetFiles(Path.Combine(sourcepath, RFN), "*.png");
        AnimationClip runClip = CreateAnimationClip(filesInRun, new DirectoryInfo(sourcepath).Name, RFN, "Run");
        runState.motion = runClip;

        AssetDatabase.CreateAsset(idleClip, Path.Combine("Assets", rootName, "Animations", new DirectoryInfo(sourcepath).Name, idleClip.name + ".anim"));
        AssetDatabase.CreateAsset(runClip, Path.Combine("Assets", rootName, "Animations", new DirectoryInfo(sourcepath).Name, runClip.name + ".anim"));

        string firstName = Path.GetFileName(filesInIdle[0]);
        string firstPath = Path.Combine("Assets", rootName, "Sprites", "characters", new DirectoryInfo(sourcepath).Name, IFN, firstName);
        Sprite first = (Sprite)AssetDatabase.LoadAssetAtPath(firstPath, typeof(Sprite));

        CharacterEntity characterEntity = CreatePrefab(out GameObject characterGO, new DirectoryInfo(sourcepath).Name, first, controller);
        CharacterDefinition characterDefinition = CreateDefinition(new DirectoryInfo(sourcepath).Name, characterEntity);
        UnityEngine.Object.DestroyImmediate(characterGO);

        ResourceManager.Instance.characterDefinitions.Add(characterDefinition);

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }

    static AnimationClip CreateAnimationClip(string[] spriteArrayPath, string characterName, string folder, string name, float _frameRate = 30)
    {
        AnimationClip clip = new AnimationClip
        {
            legacy = false,
            name = name,
            frameRate = _frameRate,
        };

        AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        animationClipSettings.loopTime = true;

        AnimationUtility.SetAnimationClipSettings(clip, animationClipSettings);

        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            path = "",
            type = typeof(SpriteRenderer),
            propertyName = "m_Sprite"
        };

        Sprite[] sprites = new Sprite[spriteArrayPath.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            string spriteName = Path.GetFileName(spriteArrayPath[i]);
            string spritePath = Path.Combine("Assets", rootName, "Sprites", "characters", characterName, folder, spriteName);
            sprites[i] = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));
        }


        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[spriteArrayPath.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe
            {
                time = i / clip.frameRate, value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyFrames);
        return clip;
    }

    static CharacterEntity CreatePrefab(out GameObject gameObject, string name, Sprite first, RuntimeAnimatorController runtimeAnimatorController)
    {
        GameObject characterGO = gameObject = new GameObject(name);
        characterGO.AddComponent<CharacterEntity>();
        characterGO.AddComponent<CharacterController>();
        characterGO.AddComponent<CharacterInput>();
        characterGO.AddComponent<CharacterCamera>();
        characterGO.AddComponent<CharacterProgress>();
        characterGO.AddComponent<RoomPlayer>();

        GameObject bodyGO = new GameObject("body");
        SpriteRenderer _renderer = bodyGO.AddComponent<SpriteRenderer>();
        Animator _animator = bodyGO.AddComponent<Animator>();
        Rigidbody2D _rigidbody2D = bodyGO.AddComponent<Rigidbody2D>();
        CircleCollider2D _circleCollider2D = bodyGO.AddComponent<CircleCollider2D>();
        bodyGO.AddComponent<CharacterAnimator>();

        _renderer.sprite = first;
        _animator.runtimeAnimatorController = runtimeAnimatorController;

        _rigidbody2D.isKinematic = true;
        _rigidbody2D.useFullKinematicContacts = true;
        _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidbody2D.freezeRotation = true;

        _circleCollider2D.radius = 1;

        bodyGO.transform.SetParent(characterGO.transform);

        string localPath = Path.Combine("Assets", rootName, "Prefabs", "Game Ready Characters", characterGO.name + ".prefab");
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        PrefabUtility.SaveAsPrefabAssetAndConnect(characterGO, localPath, InteractionMode.UserAction, out bool prefabSuccess);
        if (prefabSuccess == true)
        {
            myLogger.Log(LogType.Log, tag, $"Prefab {name} was saved successfully");
        }
        else
        {
            myLogger.Log(LogType.Error, tag, $"Prefab {name} failed to save" + prefabSuccess);
        }

        return (CharacterEntity)AssetDatabase.LoadAssetAtPath(localPath, typeof(CharacterEntity));
    }

    static CharacterDefinition CreateDefinition(string name, CharacterEntity characterEntity)
    {
        CharacterDefinition characterDefinition = ScriptableObject.CreateInstance<CharacterDefinition>();
        characterDefinition.prefab = characterEntity;

        string definitionPath = Path.Combine("Assets", rootName, "Sriptable Objects", "Character Definitions", name + ".asset");
        AssetDatabase.CreateAsset(characterDefinition, definitionPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        return characterDefinition;
    }
}
