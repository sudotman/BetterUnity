using UnityEngine;
using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;

//Inspired by Jason Storey

public class DefaultSetup : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    private string childrenPrefix;
    private int startIndex;
    [MenuItem("BetterUnity/Setup Default Project..")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<DefaultSetup>();
        window.minSize = size;
        window.maxSize = size;
    }
    private void OnGUI()
    {

        
        EditorGUILayout.HelpBox("If possible, use this straight when you create a new project to avoid confusion with existing folders. (I have been confused a lot)",MessageType.Warning);

        if (GUILayout.Button("Create Default Settings"))
        {
           
           CustomCreateDirectory("Scripts","Editor","FBX Imports","Materials","Audio Clips","Prefabs","Presets","Animations");

           Refresh();
        }
    }
    public static void CustomCreateDirectory(bool differentRoot,string root, params string[] folders)
    {
        var fullPath = Combine(dataPath, root);
        foreach (var newDirectory in folders)
            CreateDirectory(Combine(fullPath, newDirectory));
    }

    public static void CustomCreateDirectory(params string[] folders)
    {
        foreach (var newDirectory in folders)
            CreateDirectory(Combine(dataPath, newDirectory));
    }
}