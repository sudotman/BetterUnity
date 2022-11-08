using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;

public class RenameChildren : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    private string childrenPrefix;
    private int startIndex;
    [MenuItem("BetterUnity/Rename Suite",false,11)]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = size;
        window.maxSize = size;
    }
    private void OnGUI()
    {
        childrenPrefix = EditorGUILayout.TextField("Children Prefix", childrenPrefix);
        startIndex = EditorGUILayout.IntField("Start Index", startIndex);
        if (GUILayout.Button("Rename Selection"))
        {
            //Debug.Log("being called");
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int objectI = 0, i = startIndex; objectI < selectedObjects.Length; objectI++)
            {
                selectedObjects[objectI].name = $"{childrenPrefix}{i++}";
            }

        }
        if (GUILayout.Button("Rename Children of Selection"))
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int objectI = 0; objectI < selectedObjects.Length; objectI++)
            {
                Transform selectedObjectT = selectedObjects[objectI].transform;
                for (int childI = 0, i = startIndex; childI < selectedObjectT.childCount; childI++) selectedObjectT.GetChild(childI).name = $"{childrenPrefix}{i++}";
            }
        }
    }
}


//Inspired by Jason Storey

public class DefaultSetup : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    private string childrenPrefix;
    private int startIndex;
    [MenuItem("BetterUnity/Setup Default Project..",false,22)]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<DefaultSetup>();
        window.minSize = size;
        window.maxSize = size;
    }
    private void OnGUI()
    {


        EditorGUILayout.HelpBox("If possible, use this straight when you create a new project to avoid confusion with existing folders. (I have been confused a lot)", MessageType.Warning);

        if (GUILayout.Button("Create Default Settings"))
        {

            CustomCreateDirectory("Scripts", "Editor", "FBX Imports", "Materials", "Audio Clips", "Prefabs", "Presets", "Animations");

            Refresh();
        }
    }
    public static void CustomCreateDirectory(bool differentRoot, string root, params string[] folders)
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

public class SelectAllTag : ScriptableWizard
{
    [SerializeField]
    string tagName = "Player";

    [MenuItem("BetterUnity/Select all with/tag",false,0)]
    static void SelectAllOfTag()
    {
        ScriptableWizard.DisplayWizard("Select all with tag..", typeof(SelectAllTag), "Make selection");
    }

    private void OnWizardCreate()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tagName);
        Selection.objects = gos;
    }

}

public class SelectAllLayer : ScriptableWizard
{

    [SerializeField, Layer] int layer;

    [MenuItem("BetterUnity/Select all with/layer", false, 1)]
    static void SelectAllOfLayer()
    {
        ScriptableWizard.DisplayWizard("Select all with layer..", typeof(SelectAllLayer), "Make selection");
    }

    private void OnWizardCreate()
    {
        
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach(GameObject gb in gos)
        {
            if(gb.layer == layer)
            {
                temp.Add(gb);
            }
        }
        if(temp.Count < 1)
        {
            Debug.LogWarning("No objects with the specified layer found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }
    }
}

public class SelectAllName : ScriptableWizard
{

    [SerializeField] string objectName = "Cube";

    [MenuItem("BetterUnity/Select all with/name", false, 2)]
    static void SelectAllOfName()
    {
        ScriptableWizard.DisplayWizard("Select all with name..", typeof(SelectAllName), "Make Selection");
    }

    private void OnWizardCreate()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.name.Contains(objectName))
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with the specified name found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}
public class SelectAllMesh : ScriptableWizard
{

    [MenuItem("BetterUnity/Select all with/having mesh", false, 14)]
    static void SelectMeshes()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.GetComponent<MeshRenderer>() || gb.GetComponent<SkinnedMeshRenderer>())
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with meshes found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}

public class SelectAllAudio : ScriptableWizard
{

    [MenuItem("BetterUnity/Select all with/having audio source", false, 15)]
    static void SelectAudios()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.GetComponent<AudioSource>())
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with audio sources found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}

public class SelectAllCameras : ScriptableWizard
{

    [MenuItem("BetterUnity/Select all with/having cameras", false, 16)]
    static void SelectCameras()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject gb in gos)
        {
            if (gb.GetComponent<Camera>())
            {
                temp.Add(gb);
            }
        }
        if (temp.Count < 1)
        {
            Debug.LogWarning("No objects with cameras found.");
        }
        else
        {
            Selection.objects = temp.ToArray();
        }

    }
}

public class ScriptableWizards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}