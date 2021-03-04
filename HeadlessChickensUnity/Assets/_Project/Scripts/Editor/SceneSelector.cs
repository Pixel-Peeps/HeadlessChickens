using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : EditorWindow
{
    private static string[] sceneNames;
    private static EditorBuildSettingsScene[] buildScenes;

    [MenuItem("Tools/Scene Selector")]
    public static void OpenWindow()
    {
        SceneSelector window = (SceneSelector) GetWindow(typeof(SceneSelector), false, "Scene Selector");
        window.Show();
    }

    public static void OpenScene(string sceneName)
    {
        string scenePath = "Assets/_Project/Scenes/BuildScenes/" + sceneName + ".unity";
        EditorSceneManager.OpenScene(scenePath);
    }

    
    private void OnGUI()
    {
        Color32 originalColour = GUI.backgroundColor;
        GUI.backgroundColor = new Color32(136, 206, 125, 255);
        
        if (GUILayout.Button("Play Mode"))
        {
            OpenScene("DDOLScene");
            EditorApplication.EnterPlaymode();
        }
        
        GUI.backgroundColor = originalColour;

        if (GUILayout.Button("DDOLScene"))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                OpenScene("DDOLScene");
            }
        }
        
        if (GUILayout.Button("MenuScene"))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                OpenScene("MenuScene");
            }
        }
        
        if (GUILayout.Button("MainScene"))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                OpenScene("MainScene");
            }
        }
    }
}