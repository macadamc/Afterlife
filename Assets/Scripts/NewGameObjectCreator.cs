using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine.Rendering;
using ShadyPixel.Variables;
using ShadyPixel.StateMachine;
using Sirenix.OdinInspector.Internal;

public class NewGameObjectCreator : OdinEditorWindow
{
    [Title("Create New GameObject")]
    public string GameObjectName = "GameObject";
    public LayerMask layer;

    [Title("Component Options")]
    public bool useSprite = true;
    [ShowIf("useSprite")]
    public Sprite baseSprite;
    [ShowIf("useSprite")]
    public bool useZSorting = true;

    public bool useCollider = true;
    public enum ColliderType { Box, Circle };
    [ShowIf("useCollider")]
    public ColliderType colliderType;
    [ShowIf("useCollider")]
    public bool isTrigger;

    

    public bool useHealthComponent;
    public bool useMovementController;
    public bool useStateMachine;

    
    [MenuItem("Tools/ShadyPixel/New GameObject Creator")]
    private static void OpenWindow()
    {
        var window = GetWindow<NewGameObjectCreator>();
        window.Show();
        window.titleContent = new GUIContent("New GameObject", EditorIcons.Male.Active);
    }

    protected override void OnGUI()
    {
        base.OnGUI();
        GUILayout.ExpandHeight(true);
        if(GUILayout.Button("Generate Object"))
        {
            GenerateObject();
        }
    }

    private void GenerateObject()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("You are creating a GameObject in play mode. Any object created in play mode, will not be saved upon exiting.");
        }

        GameObject baseObject = new GameObject(GameObjectName);
        baseObject.layer = LayerMaskToInt(layer);

        if (useSprite)
            CreateSpriteRenderer(baseObject);

        if(useCollider)
            CreateCollider(baseObject);

        if (useHealthComponent)
            CreateHealth(baseObject);

        if (useMovementController)
            CreateMovementController(baseObject);

        if (useStateMachine)
            CreateStateMachine(baseObject);

        // closing the window
        EditorApplication.delayCall += Close;
    }

    private void CreateSpriteRenderer(GameObject baseObj)
    {
        SpriteRenderer spriteObject = new GameObject("Sprite").AddComponent<SpriteRenderer>();
        spriteObject.sprite = baseSprite;
        if (useZSorting)
        {
            spriteObject.gameObject.AddComponent<SortingGroup>();
            spriteObject.gameObject.AddComponent<UpdateZOrder>();
            SortingGroup sortingGroup = spriteObject.GetComponent<SortingGroup>();
            sortingGroup.sortingLayerName = "Entity";
        }
        spriteObject.transform.parent = baseObj.transform;
        spriteObject.gameObject.layer = LayerMaskToInt(layer);
    }

    private void CreateCollider(GameObject baseObj)
    {
        GameObject colliderObject = new GameObject("Collider");
        Collider2D col;
        if (colliderType == ColliderType.Box)
        {
            col = colliderObject.AddComponent<BoxCollider2D>();
        }
        else
        {
            col = colliderObject.AddComponent<CircleCollider2D>();
        }
        col.isTrigger = isTrigger;
        colliderObject.transform.parent = baseObj.transform;
        colliderObject.layer = LayerMaskToInt(layer);
    }

    private void CreateHealth(GameObject baseObj)
    {
        Health hp = baseObj.AddComponent<Health>();
    }

    private void CreateMovementController(GameObject baseObj)
    {
        baseObj.AddComponent<FlockingAgent>();
        Rigidbody2D rb = baseObj.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0f;
    }

    private void CreateStateMachine(GameObject baseObj)
    {
        GameObject stateMachineObj = new GameObject("State Machine");
        stateMachineObj.AddComponent<StateMachine>();

        GameObject emptyState = new GameObject("Empty State");

        emptyState.transform.parent = stateMachineObj.transform;

        stateMachineObj.transform.parent = baseObj.transform;
    }

    public static int LayerMaskToInt(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return Mathf.Max(0, layerNumber - 1);
    }


}
