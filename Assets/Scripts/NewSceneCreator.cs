using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

public class NewSceneCreator : OdinEditorWindow
{
    [Space(10)]
    [Title("Create New Scene")]
    public string NewSceneName = "NewScene";
    public string filePath = "Assets/Scenes/";
    public bool AddToBuildSettings = true;
    [Space(10)]
    [InfoBox("Optional. Will use default values if left blank.")]
    public string AreaName = "";
    public AmbientLightSettings ambientLightSettings;
    public AudioClip BgmClip;

    [MenuItem("Tools/ShadyPixel/New Scene Creator")]
    private static void OpenWindow()
    {
        var window = GetWindow<NewSceneCreator>();
        window.Show();
        window.titleContent = new GUIContent("New Scene", EditorIcons.StarPointer.Active);
    }

    protected override void OnGUI()
    {
        base.OnGUI();
        GUILayout.ExpandHeight(true);
        if (GUILayout.Button("Create Scene"))
        {
            CheckAndCreateScene();
        }
    }

    public void CheckAndCreateScene()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("Cannot create scene while in play mode. Exit play mode first.");
            return;
        }

        Scene currentActiveScene = SceneManager.GetActiveScene();
        if (currentActiveScene.isDirty)
        {
            string title = currentActiveScene.name + " Has Been Modified";
            string message = "Do you want to save the changes you made to " + currentActiveScene.path + "?\nChanges will be lost if you don't save them.";
            int option = EditorUtility.DisplayDialogComplex(title, message, "Save", "Don't Save", "Cancel");

            if (option == 0)
            {
                EditorSceneManager.SaveScene(currentActiveScene);
            }
            else if (option == 2)
            {
                return;
            }
        }

        CreateScene();
    }

    public void CreateScene()
    {
        string[] result = AssetDatabase.FindAssets("_TemplateScene");

        if (result.Length > 0)
        {
            string newScenePath = filePath + NewSceneName + ".unity";
            AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(result[0]), newScenePath);
            AssetDatabase.Refresh();
            Scene newScene = EditorSceneManager.OpenScene(newScenePath, OpenSceneMode.Single);
            if(AddToBuildSettings)
                AddSceneToBuildSettings(newScene);

            SetUpScene();

            Debug.Log("Created and Opened :" + newScenePath + ".");
            //have to use delay call because bug with odin, just waits until end of frame I think
            EditorApplication.delayCall += Close;
        }
        else
        {
            //Debug.LogError("The template scene <b>_TemplateScene</b> couldn't be found ");
            EditorUtility.DisplayDialog("Error",
                "The scene _TemplateScene was not found in Assets/Scenes folder. This scene is required by the New Scene Creator.",
                "OK");
        }
    }

    public void AddSceneToBuildSettings(Scene scene)
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;

        EditorBuildSettingsScene[] newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
        for (int i = 0; i < buildScenes.Length; i++)
        {
            newBuildScenes[i] = buildScenes[i];
        }
        newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scene.path, true);
        EditorBuildSettings.scenes = newBuildScenes;
    }

    public void SetUpScene()
    {
        if(AreaName.Length > 0)
        {
            FindObjectOfType<SetAreaName>().areaName = AreaName;
        }

        if(BgmClip != null)
        {
            FindObjectOfType<PlayBGM>().clip = BgmClip;
        }

        if(ambientLightSettings != null)
        {
            var ambientLight = FindObjectOfType<SetAmbientLight>();
            ambientLight.settings = ambientLightSettings;
            ambientLight.UpdateLight();
        }
    }

}
