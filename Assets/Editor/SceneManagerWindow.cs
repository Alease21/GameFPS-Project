using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class LevelEditorWindow : EditorWindow
{
    [MenuItem("Window/Scene Selector")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.titleContent = new GUIContent("Scene Selector");
        window.Show();
    }

    private void OnGUI()
    {
        DrawToolBar();
        DrawSceneList();
    }

    private void OnEnable()
    {
        minSize = new Vector2(300, 100); //static var in EditorWindow clas
    }

    private void DrawToolBar()
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            GUILayout.Label("Scenes in Build");
            GUILayout.FlexibleSpace();
            /*
            if (GUILayout.Button("Build Settings", EditorStyles.toolbarButton))
            {
                EditorApplication.ExecuteMenuItem("File/Build Settings...");
            }
            */
        }
    }

    private Vector2 scrollPosition;
    private void DrawSceneList()
    {
        using (var scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scrollViewScope.scrollPosition;
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                DrawSceneListItem(i, scene);
            }
        }
    }

    private void DrawSceneListItem(int i, EditorBuildSettingsScene scene)
    {
        string sceneName = Path.GetFileNameWithoutExtension(scene.path);
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label(i.ToString(), GUILayout.Width(20));
            GUILayout.Label(new GUIContent(sceneName, scene.path));
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Load"))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scene.path);
                }
            }
        }
    }
}
