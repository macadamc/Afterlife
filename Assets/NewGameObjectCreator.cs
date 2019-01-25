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

public class NewGameObjectCreator : OdinEditorWindow
{
    public enum ColliderType {Box, Circle};

    [Space(10)]
    [Title("Create New GameObject")]
    public string GameObjectName = "GameObject";
    [PreviewField]
    public Sprite baseSprite;
    public bool useZSorting;
    public ColliderType colliderType;
    public bool isTrigger;

    [MenuItem("Tools/ShadyPixel/New GameObject Creator")]
    private static void OpenWindow()
    {
        var window = GetWindow<NewGameObjectCreator>();
        window.Show();
        window.titleContent = new GUIContent("New GameObject", EditorIcons.Male.Active);
    }

    [Button("Create Object", ButtonSizes.Medium)]
    private void CreateObject()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("You are creating a GameObject in play mode. Any object created in play mode, will not be saved upon exiting.");
        }

        // setting up base object
        GameObject baseObject = new GameObject(GameObjectName);

        // setting up sprite renderer
        SpriteRenderer spriteObject = new GameObject("Sprite").AddComponent<SpriteRenderer>();
        spriteObject.sprite = baseSprite;
        if (useZSorting)
        {
            spriteObject.gameObject.AddComponent<SortingGroup>();
            spriteObject.gameObject.AddComponent<UpdateZOrder>();
            SortingGroup sortingGroup = spriteObject.GetComponent<SortingGroup>();
            sortingGroup.sortingLayerName = "Entity";
        }
        spriteObject.transform.parent = baseObject.transform;

        // setting up collider
        GameObject colliderObject = new GameObject("Collider");
        Collider2D col;
        if(colliderType == ColliderType.Box)
        {
            col = colliderObject.AddComponent<BoxCollider2D>();
        }
        else
        {
            col =colliderObject.AddComponent<CircleCollider2D>();
        }
        col.isTrigger = isTrigger;
        colliderObject.transform.parent = baseObject.transform;


        // closing the window
        EditorApplication.delayCall += Close;
    }
}
